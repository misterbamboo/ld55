using System;

public struct RotatingValue
{
    private int MaxCountInt;
    private double MaxCount;

    public double Value { get; private set; }
    public int Index => (int)Value;

    public RotatingValue(double value, int maxCount)
    {
        MaxCount = maxCount;
        MaxCountInt = maxCount;
        Value = 0;
        Value = AddInternal(Value, value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    private double AddInternal(double a, double b)
    {
        var sum = a + b;
        var signedValue = sum % MaxCount;
        var positiveValue = (signedValue + MaxCount) % MaxCount;
        return RoundDigits(positiveValue);
    }

    private int AddIntInternal(int a, int b)
    {
        var sum = a + b;
        var signedValue = sum % MaxCountInt;
        var positiveValue = (signedValue + MaxCountInt) % MaxCountInt;
        return positiveValue;
    }

    private double RoundDigits(double value)
    {
        return Math.Round(value, 4);
    }

    public RotatingValue Add(double added)
    {
        return new RotatingValue(Value + added, MaxCountInt);
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        else if (obj is int i)
        {
            return Value == i;
        }
        else if (obj is float f)
        {
            return Value == f;
        }
        else if (obj is double d)
        {
            return Value == d;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public int InvertedIndexDistanceOf(RotatingValue destination)
    {
        return -IndexDistanceOf(destination);
    }

    public int IndexDistanceOf(RotatingValue destination)
    {
        var src = Index;
        var dst = destination.Index;

        var distance = dst - src;
        var overflowDistanceLeft = -AddIntInternal(0, src - dst);
        var overFlowDistanceRight = (MaxCountInt - src) + dst;

        if (Math.Abs(distance) < Math.Abs(overflowDistanceLeft) && Math.Abs(distance) < Math.Abs(overFlowDistanceRight))
        {
            return distance;
        }
        else if (Math.Abs(overflowDistanceLeft) < Math.Abs(overFlowDistanceRight))
        {
            return overflowDistanceLeft;
        }
        else
        {
            return overFlowDistanceRight;
        }
    }

    public double DistanceOf(RotatingValue destination)
    {
        var src = Value;
        var dst = destination.Value;

        var distance = dst - src;
        var overflowDistanceLeft = -AddInternal(0, src - dst);
        var overFlowDistanceRight = (MaxCount - src) + dst;

        if (Math.Abs(distance) < Math.Abs(overflowDistanceLeft) && Math.Abs(distance) < Math.Abs(overFlowDistanceRight))
        {
            return RoundDigits(distance);
        }
        else if (Math.Abs(overflowDistanceLeft) < Math.Abs(overFlowDistanceRight))
        {
            return RoundDigits(overflowDistanceLeft);
        }
        else
        {
            return RoundDigits(overFlowDistanceRight);
        }
    }

    #region RotatingValue operators
    public static RotatingValue operator +(RotatingValue a, RotatingValue b)
    {
        return new RotatingValue(a.Value + b.Value, a.MaxCountInt);
    }

    public static RotatingValue operator -(RotatingValue a, RotatingValue b)
    {
        return new RotatingValue(a.Value - b.Value, a.MaxCountInt);
    }

    public static RotatingValue operator *(RotatingValue a, RotatingValue b)
    {
        return new RotatingValue(a.Value * b.Value, a.MaxCountInt);
    }

    public static bool operator ==(RotatingValue a, RotatingValue b)
    {
        return a.Value == b.Value;
    }

    public static bool operator !=(RotatingValue a, RotatingValue b)
    {
        return a.Value != b.Value;
    }
    #endregion

    #region int operators
    public static implicit operator int(RotatingValue value) => (int)value.Value;

    public static RotatingValue operator +(RotatingValue a, int b)
    {
        return new RotatingValue(a.Value + b, a.MaxCountInt);
    }

    public static RotatingValue operator +(int b, RotatingValue a)
    {
        return new RotatingValue(a.Value + b, a.MaxCountInt);
    }

    public static RotatingValue operator -(RotatingValue a, int b)
    {
        return new RotatingValue(a.Value - b, a.MaxCountInt);
    }

    public static RotatingValue operator -(int b, RotatingValue a)
    {
        return new RotatingValue(a.Value - b, a.MaxCountInt);
    }

    public static RotatingValue operator *(RotatingValue a, int b)
    {
        return new RotatingValue(a.Value * b, a.MaxCountInt);
    }

    public static RotatingValue operator *(int b, RotatingValue a)
    {
        return new RotatingValue(a.Value * b, a.MaxCountInt);
    }


    public static bool operator ==(RotatingValue a, int b)
    {
        return a.Value == b;
    }

    public static bool operator !=(RotatingValue a, int b)
    {
        return a.Value != b;
    }

    public static bool operator ==(int b, RotatingValue a)
    {
        return a.Value == b;
    }

    public static bool operator !=(int b, RotatingValue a)
    {
        return a.Value != b;
    }
    #endregion

    #region double operators
    public static implicit operator double(RotatingValue value) => value.Value;

    public static RotatingValue operator +(RotatingValue a, double b)
    {
        return new RotatingValue(a.Value + b, a.MaxCountInt);
    }

    public static RotatingValue operator +(double b, RotatingValue a)
    {
        return new RotatingValue(a.Value + b, a.MaxCountInt);
    }

    public static RotatingValue operator -(RotatingValue a, double b)
    {
        return new RotatingValue(a.Value - b, a.MaxCountInt);
    }

    public static RotatingValue operator -(double b, RotatingValue a)
    {
        return new RotatingValue(a.Value - b, a.MaxCountInt);
    }

    public static RotatingValue operator *(RotatingValue a, double b)
    {
        return new RotatingValue(a.Value * b, a.MaxCountInt);
    }

    public static RotatingValue operator *(double b, RotatingValue a)
    {
        return new RotatingValue(a.Value * b, a.MaxCountInt);
    }


    public static bool operator ==(RotatingValue a, double b)
    {
        return a.Value == b;
    }

    public static bool operator !=(RotatingValue a, double b)
    {
        return a.Value != b;
    }

    public static bool operator ==(double b, RotatingValue a)
    {
        return a.Value == b;
    }

    public static bool operator !=(double b, RotatingValue a)
    {
        return a.Value != b;
    }
    #endregion

    #region float operators
    public static implicit operator float(RotatingValue value) => (float)value.Value;

    public static RotatingValue operator +(RotatingValue a, float b)
    {
        return new RotatingValue(a.Value + b, a.MaxCountInt);
    }

    public static RotatingValue operator +(float b, RotatingValue a)
    {
        return new RotatingValue(a.Value + b, a.MaxCountInt);
    }

    public static RotatingValue operator -(RotatingValue a, float b)
    {
        return new RotatingValue(a.Value - b, a.MaxCountInt);
    }

    public static RotatingValue operator -(float b, RotatingValue a)
    {
        return new RotatingValue(a.Value - b, a.MaxCountInt);
    }

    public static RotatingValue operator *(RotatingValue a, float b)
    {
        return new RotatingValue(a.Value * b, a.MaxCountInt);
    }

    public static RotatingValue operator *(float b, RotatingValue a)
    {
        return new RotatingValue(a.Value * b, a.MaxCountInt);
    }


    public static bool operator ==(RotatingValue a, float b)
    {
        return a.Value == b;
    }

    public static bool operator !=(RotatingValue a, float b)
    {
        return a.Value != b;
    }

    public static bool operator ==(float b, RotatingValue a)
    {
        return a.Value == b;
    }

    public static bool operator !=(float b, RotatingValue a)
    {
        return a.Value != b;
    }
    #endregion
}
