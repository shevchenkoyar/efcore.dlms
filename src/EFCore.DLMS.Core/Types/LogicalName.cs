using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace EFCore.DLMS.Core.Types;

/// <summary>
/// Represents the DLMS Logical Name (OBIS code) type, consisting of 6 octets.
/// </summary>
[DebuggerDisplay("{ToString()}")]
[StructLayout(LayoutKind.Sequential)]
public readonly struct LogicalName 
    : IEquatable<LogicalName>, 
        IEquatable<string>, 
        IEquatable<byte[]>
{
    private readonly byte _a;
    private readonly byte _b;
    private readonly byte _c;
    private readonly byte _d;
    private readonly byte _e;
    private readonly byte _f;

    /// <summary>
    /// Gets the first octet.
    /// </summary>
    [Pure]
    public byte A => _a;
    
    /// <summary>
    /// Gets the second octet.
    /// </summary>
    [Pure]
    public byte B => _b;
    
    /// <summary>
    /// Gets the third octet.
    /// </summary>
    [Pure]
    public byte C => _c;
    
    /// <summary>
    /// Gets the fourth octet.
    /// </summary>
    [Pure]
    public byte D => _d;
    
    /// <summary>
    /// Gets the fifth octet.
    /// </summary>
    [Pure]
    public byte E => _e;
    
    /// <summary>
    /// Gets the sixth octet.
    /// </summary>
    [Pure]
    public byte F => _f;

    /// <summary>
    /// Gets the octet by index (0..5).
    /// </summary>
    [Pure]
    public byte this[int index] => index switch
    {
        0 => _a,
        1 => _b,
        2 => _c,
        3 => _d,
        4 => _e,
        5 => _f,
        _ => throw new IndexOutOfRangeException()
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="LogicalName"/> struct from a span of bytes.
    /// </summary>
    /// <param name="value">A span containing exactly 6 bytes representing the Logical Name.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> does not contain exactly 6 bytes.</exception>
    public LogicalName(ReadOnlySpan<byte> value)
    {
        if (value.Length != 6)
            throw new ArgumentException("LogicalName must contain exactly 6 bytes.", nameof(value));
        _a = value[0];
        _b = value[1];
        _c = value[2];
        _d = value[3];
        _e = value[4];
        _f = value[5];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LogicalName"/> struct from an OBIS code string.
    /// </summary>
    /// <param name="obis">A string representing the OBIS code, consisting of 6 parts separated by '.'.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="obis"/> is null or whitespace.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="obis"/> does not have 6 parts or contains invalid byte values.</exception>
    public LogicalName(string obis)
    {
        if (string.IsNullOrWhiteSpace(obis))
            throw new ArgumentNullException(nameof(obis));
        var parts = obis.Split('.');
        if (parts.Length != 6)
            throw new ArgumentException("OBIS code must have 6 parts separated by dots.", nameof(obis));
        if (!byte.TryParse(parts[0], out _a) ||
            !byte.TryParse(parts[1], out _b) ||
            !byte.TryParse(parts[2], out _c) ||
            !byte.TryParse(parts[3], out _d) ||
            !byte.TryParse(parts[4], out _e) ||
            !byte.TryParse(parts[5], out _f))
            throw new ArgumentException("OBIS code contains invalid byte values.", nameof(obis));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LogicalName"/> struct from 6 bytes.
    /// </summary>
    public LogicalName(byte a, byte b, byte c, byte d, byte e, byte f)
    {
        _a = a;
        _b = b;
        _c = c;
        _d = d;
        _e = e;
        _f = f;
    }

    /// <summary>
    /// Returns a copy of the internal byte array representing the Logical Name.
    /// </summary>
    /// <returns>A byte array of length 6.</returns>
    [Pure]
    public byte[] GetBytes() => [_a, _b, _c, _d, _e, _f];

    /// <summary>
    /// Returns a copy of the internal byte array representing the Logical Name.
    /// </summary>
    /// <returns>A byte array of length 6.</returns>
    [Pure]
    public byte[] ToArray() => GetBytes();

    /// <summary>
    /// Returns the string representation of the Logical Name in OBIS code format (dot-separated).
    /// </summary>
    /// <returns>A string representing the Logical Name as an OBIS code.</returns>
    [Pure]
    public override string ToString() => $"{_a}.{_b}.{_c}.{_d}.{_e}.{_f}";

    /// <summary>
    /// Determines whether the specified object is equal to the current LogicalName instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current LogicalName.</param>
    /// <returns><c>true</c> if the specified object is a LogicalName and has the same value; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
    {
        return obj switch
        {
            LogicalName other => Equals(other),
            string str => Equals(str),
            byte[] arr => Equals(arr),
            _ => false
        };
    }

    /// <summary>
    /// Determines whether the specified <see cref="LogicalName"/> is equal to the current instance.
    /// </summary>
    /// <param name="other">The <see cref="LogicalName"/> to compare with the current instance.</param>
    /// <returns><c>true</c> if the specified <see cref="LogicalName"/> is equal to the current instance; otherwise, <c>false</c>.</returns>
    [Pure]
    public bool Equals(LogicalName other)
    {
        return _a == other._a && _b == other._b && _c == other._c && _d == other._d && _e == other._e && _f == other._f;
    }

    /// <summary>
    /// Determines whether the specified OBIS code string is equal to the current instance.
    /// </summary>
    /// <param name="obj">The OBIS code string to compare with the current instance.</param>
    /// <returns><c>true</c> if the specified string represents the same Logical Name; otherwise, <c>false</c>.</returns>
    [Pure]
    public bool Equals(string? obj)
    {
        if (obj is null) return false;
        var parts = obj.Split('.');
        if (parts.Length != 6) return false;
        return byte.TryParse(parts[0], out var a) && a == _a &&
               byte.TryParse(parts[1], out var b) && b == _b &&
               byte.TryParse(parts[2], out var c) && c == _c &&
               byte.TryParse(parts[3], out var d) && d == _d &&
               byte.TryParse(parts[4], out var e) && e == _e &&
               byte.TryParse(parts[5], out var f) && f == _f;
    }

    /// <summary>
    /// Determines whether the specified byte array is equal to the current instance.
    /// </summary>
    /// <param name="other">The byte array to compare with the current instance.</param>
    /// <returns><c>true</c> if the specified array represents the same Logical Name; otherwise, <c>false</c>.</returns>
    [Pure]
    public bool Equals(byte[]? other)
    {
        if (other is null || other.Length != 6) return false;
        return _a == other[0] && _b == other[1] && _c == other[2] && _d == other[3] && _e == other[4] && _f == other[5];
    }

    /// <summary>
    /// Returns a hash code for the LogicalName.
    /// </summary>
    /// <returns>An integer hash code.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(_a, _b, _c, _d, _e, _f);

    /// <summary>
    /// Determines whether two <see cref="LogicalName"/> instances are equal.
    /// </summary>
    /// <param name="left">The first <see cref="LogicalName"/> to compare.</param>
    /// <param name="right">The second <see cref="LogicalName"/> to compare.</param>
    /// <returns><c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(LogicalName left, LogicalName right) => left.Equals(right);
    
    public static bool operator ==(LogicalName left, string right) => left.Equals(right);
    public static bool operator !=(LogicalName left, string right) => left.Equals(right);

    /// <summary>
    /// Determines whether two <see cref="LogicalName"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="LogicalName"/> to compare.</param>
    /// <param name="right">The second <see cref="LogicalName"/> to compare.</param>
    /// <returns><c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(LogicalName left, LogicalName right) => !(left == right);

    /// <summary>
    /// Parses the specified OBIS code string into a <see cref="LogicalName"/>.
    /// </summary>
    /// <param name="obis">The OBIS code string.</param>
    /// <returns>The parsed <see cref="LogicalName"/>.</returns>
    /// <exception cref="FormatException">Thrown if the string is not a valid OBIS code.</exception>
    public static LogicalName Parse(string obis)
    {
        if (TryParse(obis, out var result))
            return result;
        throw new FormatException($"Invalid OBIS code: '{obis}'");
    }

    /// <summary>
    /// Tries to parse the specified OBIS code string into a <see cref="LogicalName"/>.
    /// </summary>
    /// <param name="obis">The OBIS code string.</param>
    /// <param name="result">When this method returns, contains the parsed <see cref="LogicalName"/>, if the parsing succeeded, or the default value if parsing failed.</param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    public static bool TryParse(string? obis, out LogicalName result)
    {
        result = default;
        if (string.IsNullOrWhiteSpace(obis)) return false;
        var parts = obis.Split('.');
        if (parts.Length != 6) return false;
        if (!byte.TryParse(parts[0], out var a) ||
            !byte.TryParse(parts[1], out var b) ||
            !byte.TryParse(parts[2], out var c) ||
            !byte.TryParse(parts[3], out var d) ||
            !byte.TryParse(parts[4], out var e) ||
            !byte.TryParse(parts[5], out var f))
            return false;
        result = new LogicalName(a, b, c, d, e, f);
        return true;
    }

    /// <summary>
    /// Explicit conversion from byte array to <see cref="LogicalName"/>.
    /// </summary>
    /// <param name="bytes">A byte array of length 6.</param>
    /// <exception cref="ArgumentNullException">Thrown if the array is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the array is not of length 6.</exception>
    public static explicit operator LogicalName(byte[] bytes)
    {
        if (bytes == null)
            throw new ArgumentNullException(nameof(bytes));
        return new LogicalName(bytes);
    }

    /// <summary>
    /// Implicit conversion from tuple of 6 bytes to <see cref="LogicalName"/>.
    /// </summary>
    public static implicit operator LogicalName((byte, byte, byte, byte, byte, byte) tuple)
        => new LogicalName(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6);

    /// <summary>
    /// Returns the LogicalName as a tuple of 6 bytes.
    /// </summary>
    [Pure]
    public (byte, byte, byte, byte, byte, byte) ToTuple() => (_a, _b, _c, _d, _e, _f);

    /// <summary>
    /// Gets the LogicalName with all octets set to 0.
    /// </summary>
    [Pure]
    public static LogicalName Zero => new(0, 0, 0, 0, 0, 0);

    /// <summary>
    /// Gets the LogicalName with all octets set to 255.
    /// </summary>
    [Pure]
    public static LogicalName Max => new(255, 255, 255, 255, 255, 255);

    /// <summary>
    /// Determines whether the beginning of this LogicalName matches the specified prefix.
    /// </summary>
    [Pure]
    public bool StartsWith(params byte[]? prefix)
    {
        if (prefix == null || prefix.Length > 6) return false;
        for (var i = 0; i < prefix.Length; i++)
            if (this[i] != prefix[i]) return false;
        return true;
    }

    /// <summary>
    /// Determines whether the end of this LogicalName matches the specified suffix.
    /// </summary>
    [Pure]
    public bool EndsWith(params byte[]? suffix)
    {
        if (suffix == null || suffix.Length > 6) return false;
        var offset = 6 - suffix.Length;
        for (var i = 0; i < suffix.Length; i++)
            if (this[offset + i] != suffix[i]) return false;
        return true;
    }
    
    public static implicit operator LogicalName(string x)
    {
        return new LogicalName(x);
    }
}