using OpenTK.Mathematics;

namespace PieperEngine.Physics.Collisions
{
    public enum ResolutionAxis { X, Y }

    public static class CollisionSystem
    {
        private static readonly Dictionary<Vector2i, List<Collider>> _spatialHashMap = [];
        private static readonly List<Collider> _colliders = [];
        private static ResolutionAxis _resolutionAxis = ResolutionAxis.X;

        public static readonly float CellSize = 512f;

        public static bool ShowHitboxes { get; private set; } = true;

        public static void Register(Collider collider) => _colliders.Add(collider);

        public static void AddToSpatialHashMap(Collider collider)
        {
            Vector2[] points = collider.GetPoints();
            Vector2 aabbMax = AABBMax(points);
            Vector2 aabbMin = AABBMin(points);

            Vector2i minCell = new(
                x: (int)Math.Floor(aabbMin.X / CellSize),
                y: (int)Math.Floor(aabbMin.Y / CellSize)
            );

            Vector2i maxCell = new(
                x: (int)Math.Floor(aabbMax.X / CellSize),
                y: (int)Math.Floor(aabbMax.Y / CellSize)
            );

            Vector2i cellPosition = new();
            for (int x = minCell.X; x <= maxCell.X; x++)
            {
                for (int y = minCell.Y; y <= maxCell.Y; y++)
                {
                    cellPosition.X = x;
                    cellPosition.Y = y;
                    AddColliderToCell(collider, cellPosition);
                }
            }
        }

        private static Vector2 AABBMax(Vector2[] points)
        {
            Vector2 aabbMax = new(float.MinValue);
            foreach (var vertex in points)
            {
                if (vertex.X > aabbMax.X) aabbMax.X = vertex.X;
                if (vertex.Y > aabbMax.Y) aabbMax.Y = vertex.Y;
            }

            return aabbMax;
        }

        private static Vector2 AABBMin(Vector2[] points)
        {
            Vector2 aabbMin = new(float.MaxValue);
            foreach (var vertex in points)
            {
                if (vertex.X < aabbMin.X) aabbMin.X = vertex.X;
                if (vertex.Y < aabbMin.Y) aabbMin.Y = vertex.Y;
            }

            return aabbMin;
        }

        private static void AddColliderToCell(Collider collider, Vector2i cell)
        {
            if (!_spatialHashMap.TryGetValue(cell, out List<Collider>? value))
            {
                value = [];
                _spatialHashMap[cell] = value;
            }

            value.Add(collider);
            collider.Cell = cell;
        }

        public static void Reset()
        {
            _colliders.Clear();
            _spatialHashMap.Clear();
        }

        public static void Update(ResolutionAxis axis)
        {
            _resolutionAxis = axis;
            _spatialHashMap.Clear();
            foreach (var collider in _colliders) 
            {
                collider.Update();
                AddToSpatialHashMap(collider);
            }

            foreach (var colliders in _spatialHashMap.Values)
            {
                CheckForCollisions(colliders);
            }
        }

        private static void CheckForCollisions(List<Collider> colliders)
        {
            while (colliders.Count >= 2)
            {
                var aCollider = colliders[0];
                for (int i = 1; i < colliders.Count; i++)
                {
                    var bCollider = colliders[i];

                    if (!aCollider.Enabled || !bCollider.Enabled) continue;

                    Vector2[] a = aCollider.GetPoints();
                    Vector2[] b = bCollider.GetPoints();
                    if (!(IsCollidingPolygonPolygon(a, b, out Vector2 resA) &&
                        IsCollidingPolygonPolygon(b, a, out Vector2 resB))) continue;

                    var entityA = aCollider.Entity;
                    var entityB = bCollider.Entity;

                    if (aCollider.Type == ColliderType.Trigger || bCollider.Type == ColliderType.Trigger)
                    {
                        entityA.OnTriggerEnter(bCollider);
                        entityB.OnTriggerEnter(aCollider);
                        continue;
                    }

                    Vector3 axis = (_resolutionAxis == ResolutionAxis.X)
                            ? Vector3.UnitX
                            : Vector3.UnitY;

                    Vector3 resolution = (resA.LengthSquared < resB.LengthSquared)
                            ? new Vector3(resA.X, resA.Y, 0)
                            : new Vector3(-resB.X, -resB.Y, 0);

                    if (aCollider.Active)
                    {
                        entityA.Transform.Position += axis * resolution;

                        var collisionArgs = new CollisionEventArgs()
                        {
                            Other = bCollider,
                            Axis = _resolutionAxis
                        };

                        entityA.OnCollisionEnter(collisionArgs);
                    }
                    else if (bCollider.Active)
                    {
                        resolution *= -1;
                        entityB.Transform.Position += axis * resolution;

                        var collisionArgs = new CollisionEventArgs()
                        {
                            Other = aCollider,
                            Axis = _resolutionAxis
                        };

                        entityB.OnCollisionEnter(collisionArgs);
                    }
                }

                colliders.RemoveAt(0);
            }
        }

        private static bool IsCollidingPolygonPolygon(Vector2[] a, Vector2[] b, out Vector2 resolution)
        {
            Vector2 minIntersection = new(float.MaxValue);
            for (int i = 0; i < a.Length; i++)
            {
                Vector2 normal = GetPositiveNormal(a[i], a[(i + 1) % a.Length]);

                ProjectPolygon(a, normal, out float tMinA, out float tMaxA);
                ProjectPolygon(b, normal, out float tMinB, out float tMaxB);

                if (!ProjectionsIntersect(tMinA, tMaxA, tMinB, tMaxB))
                {
                    resolution = Vector2.Zero;
                    return false;
                }

                Vector2 min = IntersectionMin(normal, tMinA, tMaxA, tMinB, tMaxB);
                if (min.Length < minIntersection.Length)
                {
                    minIntersection = min;
                }
            }

            resolution = minIntersection;
            return true;
        }

        private static void ProjectPolygon(Vector2[] points, Vector2 axis, out float tMin, out float tMax)
        {
            float dot = Vector2.Dot(points[0], axis);
            tMin = tMax = dot;

            for (int i = 1; i < points.Length; i++)
            {
                dot = Vector2.Dot(points[i], axis);
                if (dot < tMin) tMin = dot;
                if (dot > tMax) tMax = dot;
            }
        }

        private static Vector2 GetPositiveNormal(Vector2 startPoint, Vector2 endPoint)
        {
            Vector2 original = endPoint - startPoint;
            Vector2 positiveNormal = new (-original.Y, original.X);
            return Vector2.Normalize(positiveNormal);
        }

        private static bool ProjectionsIntersect(float tMinA, float tMaxA, float tMinB, float tMaxB)
        {
            return tMaxA > tMinB && tMaxB > tMinA;
        }

        private static Vector2 IntersectionMin(Vector2 axis, float tMinA, float tMaxA, float tMinB, float tMaxB)
        {
            float tLeft = tMaxA - tMinB;
            float tRight = tMaxB - tMinA;

            if (tRight < tLeft) return tRight * axis;
            else return -tLeft * axis;
        }

        public static bool BoxCast(Collider aCollider, Vector2 displace, LayerMask filter)
        {
            Vector2[] a = aCollider.GetPoints();
            Vector2 maxA = AABBMax(a);
            Vector2 minA = AABBMin(a);
            for (int i = 0; i < a.Length; i++)
            {
                a[i] += displace;
            }

            foreach (var bCollider in _colliders)
            {
                if (!bCollider.Enabled || bCollider == aCollider) continue;
                if ((bCollider.Entity.Bitmask.Value & filter.Value) == 0) continue;

                Vector2[] b = bCollider.GetPoints();
                Vector2 maxB = AABBMax(b);
                Vector2 minB = AABBMin(b);
                if (!(ProjectionsIntersect(minA.X, maxA.X, minB.X, maxB.X)
                   || ProjectionsIntersect(minA.Y, maxA.Y, minB.Y, maxB.Y)))
                {
                    continue;
                }

                if (IsCollidingPolygonPolygon(a, b, out _) &&
                    IsCollidingPolygonPolygon(b, a, out _))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
