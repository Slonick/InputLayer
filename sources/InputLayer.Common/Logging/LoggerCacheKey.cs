using System;

namespace InputLayer.Common.Logging
{
    internal struct LoggerCacheKey : IEquatable<LoggerCacheKey>
    {
        public Type ConcreteType { get; }

        public string Name { get; }

        public LoggerCacheKey(string name, Type concreteType)
        {
            this.Name = name;
            this.ConcreteType = concreteType;
        }

        public override bool Equals(object obj)
        {
            if (obj is LoggerCacheKey other)
            {
                return this.Equals(other);
            }

            return false;
        }

        public bool Equals(LoggerCacheKey other)
            => this.ConcreteType == other.ConcreteType && string.Equals(other.Name, this.Name, StringComparison.Ordinal);

        public override int GetHashCode()
            => this.ConcreteType.GetHashCode() ^ this.Name.GetHashCode();
    }
}