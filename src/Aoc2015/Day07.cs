namespace Aoc2015;

public class Day07 : BaseDay
{
    private Dictionary<Wire, IExpression> _wires;

    public Day07()
    {
        _wires = ParseInput();
    }

    public override ValueTask<string> Solve_1()
    {
        var aWire = _wires[new Wire("a")];
        return new ValueTask<string>(aWire.Eval().ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        _wires = ParseInput(46065);
        return new ValueTask<string>(_wires[new Wire("a")].Eval().ToString());
    }

    private Dictionary<Wire, IExpression> ParseInput(ushort? overrideB = null)
    {
        var lines = File.ReadAllLines(InputFilePath);
        var wires = new Dictionary<Wire, IExpression>();

        foreach (var line in lines)
        {
            var tokens = line.Split("->", StringSplitOptions.RemoveEmptyEntries);
            var wire = new Wire(tokens[1].Trim());

            var leftPart = tokens[0].Trim().ToLower();

            // hacky solution but works... to mitigate that issue expression trees should be built on the fly
            if (wire.Name == "b" && overrideB != null)
                wires[wire] = new ConstantExpression((ushort)overrideB);
            else
                wires[wire] = GetExpression(leftPart, wires);
        }

        return wires;
    }

    private IExpression GetExpression(string part, Dictionary<Wire, IExpression> wires)
    {
        if (part.Contains("not"))
        {
            var numberOrWire = part.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1];
            return new NotExpression(GetNumberOrWireExpression(numberOrWire, wires));
        }

        if (part.Contains("and"))
        {
            var left = part.Split("and", StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            var right = part.Split("and", StringSplitOptions.RemoveEmptyEntries)[1].Trim();
            return new AndExpression(GetNumberOrWireExpression(left, wires), GetNumberOrWireExpression(right, wires));
        }

        if (part.Contains("or"))
        {
            var left = part.Split("or", StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            var right = part.Split("or", StringSplitOptions.RemoveEmptyEntries)[1].Trim();
            return new OrExpression(GetNumberOrWireExpression(left, wires), GetNumberOrWireExpression(right, wires));
        }

        if (part.Contains("lshift"))
        {
            var left = part.Split("lshift", StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            var right = part.Split("lshift", StringSplitOptions.RemoveEmptyEntries)[1].Trim();
            return new LShiftExpression(GetNumberOrWireExpression(left, wires), ushort.Parse(right));
        }

        if (part.Contains("rshift"))
        {
            var left = part.Split("rshift", StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            var right = part.Split("rshift", StringSplitOptions.RemoveEmptyEntries)[1].Trim();
            return new RShiftExpression(GetNumberOrWireExpression(left, wires), ushort.Parse(right));
        }

        return GetNumberOrWireExpression(part, wires);
    }

    private IExpression GetNumberOrWireExpression(string numberOrWire, Dictionary<Wire, IExpression> wires)
    {
        if (ushort.TryParse(numberOrWire, out var number))
            return new ConstantExpression(number);
        return new WireExpression(new Wire(numberOrWire), wires);
    }

    private interface IExpression
    {
        ushort Eval();
    }

    private class ConstantExpression : IExpression
    {
        private ushort Constant { get; set; }

        public ConstantExpression(ushort constant)
        {
            Constant = constant;
        }

        public ushort Eval()
        {
            return Constant;
        }
    }


    private class AndExpression : IExpression
    {
        private IExpression Left { get; set; }
        private IExpression Right { get; set; }

        public AndExpression(IExpression left, IExpression right)
        {
            Left = left;
            Right = right;
        }

        public ushort Eval()
        {
            return (ushort)(Left.Eval() & Right.Eval());
        }
    }

    private class OrExpression : IExpression
    {
        private IExpression Left { get; set; }
        private IExpression Right { get; set; }

        public OrExpression(IExpression left, IExpression right)
        {
            Left = left;
            Right = right;
        }

        public ushort Eval()
        {
            return (ushort)(Left.Eval() | Right.Eval());
        }
    }

    private class RShiftExpression : IExpression
    {
        private IExpression Left { get; set; }
        private ushort Shift { get; set; }

        public RShiftExpression(IExpression left, ushort shift)
        {
            Left = left;
            Shift = shift;
        }

        public ushort Eval()
        {
            return (ushort)(Left.Eval() >> Shift);
        }
    }


    private class LShiftExpression : IExpression
    {
        private IExpression Left { get; set; }
        private ushort Shift { get; set; }

        public LShiftExpression(IExpression left, ushort shift)
        {
            Left = left;
            Shift = shift;
        }

        public ushort Eval()
        {
            return (ushort)(Left.Eval() << Shift);
        }
    }

    private class NotExpression : IExpression
    {
        private IExpression Expression { get; set; }

        public NotExpression(IExpression expression)
        {
            Expression = expression;
        }

        public ushort Eval()
        {
            return (ushort)(~Expression.Eval());
        }
    }

    private class WireExpression : IExpression
    {
        private Wire Wire { get; set; }

        private Dictionary<Wire, IExpression> Wires { get; set; }

        public WireExpression(Wire wire, Dictionary<Wire, IExpression> wires)
        {
            Wire = wire;
            Wires = wires;
        }

        public ushort Eval()
        {
            if (Wire.Signal != null)
                return (ushort)Wire.Signal;
            var signal = Wires[Wire].Eval();
            Wire.SetSignal(signal);
            return signal;
        }
    }

    public record Wire(string Name)
    {
        public ushort? Signal { get; private set; }

        public void SetSignal(ushort signal)
        {
            Signal = signal;
        }

        public void ResetSignal()
        {
            Signal = null;
        }

        public virtual bool Equals(Wire? other)
        {
            return Name == other?.Name;
        }
    }
}