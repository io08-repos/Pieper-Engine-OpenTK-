namespace PieperEngine.Physics
{
    public readonly struct LayerMask
    {
        private readonly ulong _bitmask;
        public readonly ulong Value => _bitmask;

        private static readonly Dictionary<string, int> _layers = [];

        public LayerMask(string layerName)
        {
            if (!_layers.TryGetValue(layerName, out int layer))
            {
                layer = _layers.Count;
                if (layer > 64)
                {
                    throw new InvalidOperationException("You ran out of layers; limit is 64!");
                }

                _layers[layerName] = layer;
            }

            _bitmask = 1UL << layer;
        }

        public LayerMask(ulong value)
        {
            _bitmask = value;
        }

        public static LayerMask GetMask(params string[] layerNames)
        {
            ulong mask = 0;
            foreach (var layerName in layerNames)
            {
                if (!_layers.TryGetValue(layerName, out int layer))
                {
                    throw new ArgumentException($"Layer '{layerName}' does not exist.");
                }

                mask |= 1UL << layer;
            }

            return new LayerMask(mask);
        }

        public bool Contains(LayerMask layer) => (_bitmask & layer.Value) != 0;
    }
}
