# Hef.Math.Interpreter

An interpreter that takes a math formula (string) as input, breaks it into a chain of values and operations, and outputs a numeric result.
It also handles variable declarations as key/value pairs.

## Getting Started

### Prerequisites

This software is based on the **.Net 2.0 Framework** and has no other dependencies.

### Installing

There is different options to get it into your own projet.

#### Github

Clone the repository, or copy/paste the code into your solution.

```
git clone https://github.com/fsegaud/Hef.Math.Interpreter.git
```

#### NuGet Package

You can also install the correpsonding _NuGet_ package at [https://www.nuget.org/packages/Hef.Math.Interpreter](https://www.nuget.org/packages/Hef.Math.Interpreter)

Or install it using the _NuGet_ console.

```
Install-Package Hef.Math.Interpreter -Version 0.1.0-alpha 
```

### Examples

The interpreter accepts multiple formula coding conventions. You can uses spaces, or not. You can use parenthesis, or not. It's up to you.

Here is the simplest example.

```csharp
Interpreter interpreter = new Interpreter();
double result = interpreter.Calculate("sqrt(4) + 2"); // -> 4
// "sqrt 4 + 2" or "sqrt4+2" would work as well
```

The following example highlights the use of manually registered variables.

```csharp
Interpreter interpreter = new Interpreter();
interpreter.SetVar("foo", 1d);
interpreter.SetVar("bar", 2d);
double result = interpreter.Calculate("($foo + $bar) * 2"); // -> 6
```

The following example highlights the use of `Hef.Math.IInterpreterContext`, that allow the interpreter to access variables provided by other objects.

```csharp
Interpreter interpreter = new Interpreter(new Player());
double result = interpreter.Calculate("$level - 1"); // -> 9

class Player : Hef.Math.IInterpreterContext
{
    private int level = 10;

    public bool TryGetVariable(string name, out double value)
    {
        value = 0d;

        if (name == "level")
        {
            value = this.level;
            return true;
        }

        return false;
    }
}
```

### Handled Operations

#### Basic Operators

| Symbol | Operation   | Comment                            | Version |
|:------:|-------------|------------------------------------|:-------:|
| `±`    | Sign Change | -1 should be written ±1 (atl+0177) | 0.1.0   |
| `+`    | Addition    |                                    | 0.1.0   |
| `-`    | Subtraction |                                    | 0.1.0   |
| `*`    | Product     |                                    | 0.1.0   |
| `/`    | Division    |                                    | 0.1.0   |
| `%`    | Modulo      |                                    | 0.1.0   |
| `^`    | Power       |                                    | 0.1.0   |
| `sqrt` | Square Root |                                    | 0.1.0   |

#### Advanced Operators

| Symbol  | Operation      | Comment | Version |
|:-------:|----------------|---------|:-------:|
| `abs`   | Absolute Value |         | 0.1.0   |
| `round` | Round          |         | 0.1.0   |
| `min`   | Minimum        |         | 0.1.0   |
| `max`   | Maximum        |         | 0.1.0   |

#### Comparison Operators

| Symbol       | Operation        | Comment                    | Version |
|:------------:|------------------|----------------------------|:-------:|
| `==` or `eq` | Equal            | 1 == 1 -> 1, 1 eq 2 -> 0   | 0.1.0   |
| `gt`         | Greater Than     | 1 gt 1 -> 0, 1 gt 2 -> 1   | 0.1.0   |
| `gte`        | Greater Or Equal | 1 gte 0 -> 0, 1 gte 1 -> 1 | 0.1.0   |
| `lt`         | Less Than        | 1 lt 1 -> 0, 1 lt 2 -> 1   | 0.1.0   |
| `lte`        | Less Or Equal    | 1 lte 1 -> 1, 1 lte 0 -> 0 | 0.1.0   |

#### Logical Operators

| Symbol       | Operation        | Comment                    | Version |
|:------------:|------------------|----------------------------|:-------:|
| `!`          | Not              | !0 -> 1, !1 -> 0           | 0.1.0   |
| `&` or `and` | And              | true & true -> true        | 0.1.1   |
| `\|` or `or` | Or               | true & false -> true       | 0.1.1   |

#### Trigonometry

| Symbol   | Operation          | Comment                     | Version |
|:--------:|--------------------|-----------------------------|:-------:|
| `cos`    | Cosine             |                             | 0.1.0   |
| `sin`    | Sine               |                             | 0.1.0   |
| `tan`    | Tangent            |                             | 0.1.0   |
| `acos`   | Arccosine          |                             | 0.1.1   |
| `asin`   | Arcsine            |                             | 0.1.1   |
| `atan`   | Arctangent         |                             | 0.1.1   |
| `cosh`   | Hyperbolic Cosine  |                             | 0.1.1   |
| `sinh`   | Hyperbolic Sine    |                             | 0.1.1   |
| `tanh`   | Hyperbolic Tangent |                             | 0.1.1   |
| `degrad` | Deg2Rad            | Converts degrees to radians | 0.1.1   |
| `raddeg` | Rad2Deg            | Converts radians to degrees | 0.1.1   |

#### Randomization

| Symbol     | Operation | Comment         | Version |
|:----------:|-----------|-----------------|:-------:|
| `rand`     | Random    | rand 5 -> [0,5] | 0.1.0   |
| `d` or `D` | Dice      | 2d6 -> [2,12]   | 0.1.0   |

#### Constants

| Symbol  | Operation     | Comment | Version |
|:-------:|---------------|---------|:-------:|
| `pi`    | Value of PI   | 3.14... | 0.1.0   |
| `true`  | Boolean true  | 1       | 0.1.1   |
| `false` | Boolean false | 0       | 0.1.1   |


## Contributing

### How To Define Additional Operations

The interpreter handles the basic mathmatical operations. If you need more, everything you have to do is open the _Interpreter.Operators.cs_ file, and implement the following steps :

1. Add your new operator to the `Operator` enum.
2. Add the corresponding `OperatorDescriptor` in the `operators` dictionary, along with its type and priority.
3. Implement the operation code in the `ComputeOperation()` function.
 
The following example show the implementation of an operator that halves an operand. Its symbol will be `#`.

```csharp
private enum Operator
{
    /* ... */
    Half
}

operators = new System.Collections.Generic.Dictionary<string, OperatorDescriptor>
{
    /* ... */
    { "#", new OperatorDescriptor(Operator.Half, OperatorType.Unary, 50) }
}

private static double ComputeOperation(double left, double right, Operator op)
{
    switch (op)
    {
        /* ... */
        case Operator.Half:
            return left  * .5d;
    }
}
```

> **DO** make a pull request if you want. More operators makes the interpreter better :smile:

## License

This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/fsegaud/Hef.Math.Interpreter/blob/master/LICENSE.md) file for details
