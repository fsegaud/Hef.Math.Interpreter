# Hef.Math.Interpreter

[![GitHub release](https://img.shields.io/github/release/fsegaud/Hef.Math.Interpreter.svg)](https://github.com/fsegaud/Hef.Math.Interpreter/releases) [![NuGet](https://img.shields.io/nuget/v/Hef.Math.Interpreter.svg)](https://www.nuget.org/packages/Hef.Math.Interpreter) [![license](https://img.shields.io/github/license/fsegaud/Hef.Math.Interpreter.svg)](https://github.com/fsegaud/Hef.Math.Interpreter/blob/master/LICENSE.md) [![Say Thanks!](https://img.shields.io/badge/Say%20Thanks-!-1EAEDB.svg)](https://saythanks.io/to/fseg)

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
Install-Package Hef.Math.Interpreter -Version 1.1.1
```

### Examples

The interpreter accepts two notation styles. Operations can be written as functions with parenthesis and arguments, or like regular operations with a symbol. There is actually no difference between functions and symbols in the implementation.
For instance, the addition can be written `add(1, 2)` or `1 + 2`. Or even `add 1 2` or `+(1, 2)` if you like it.

The complete list of handled operations is availablable at [Annex - Handled Operations](#annex---handled-operations).

Here is a simple example.

```csharp
Interpreter interpreter = new Interpreter();
double result = interpreter.Calculate("sqrt(4) + 2"); // -> 4
```

The following example highlights the use of manually registered local and global variables.

```csharp
Interpreter interpreter = new Interpreter();
Interpreter.SetGlobalVar("foo", 1d);
interpreter.SetVar("bar", 2d);
double result = interpreter.Calculate("($foo + $bar) * 2"); // -> 6
```

The following example highlights the use of `Hef.Math.IInterpreterContext`, that allows the interpreter to access variables provided by other objects.

```csharp
Interpreter interpreter = new Interpreter();
interpreter.SetContext("player", new Player()));
double result = interpreter.Calculate("$player.level - 1"); // -> 9

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

### Note About Caching

Each time a formula is calculated, the interpreter has to breaks the formula into nodes and build a tree of operations. This is a time-consumming process.

In order to make it faster, each time a new formula is processed, the intrepreder will keep the generated tree in memory (up to 64). So if the same formula is used again, the tree will be reused, and only the mathematical operations will be recomputed.

If for some reason the cache has to be manually cleared, the `Interpreter` provides a function to do so.

```csharp
Interpreter.ForceClearCache();
```

## Contributing

### How To Define Additional Operations

The interpreter handles the basic mathmatical operations. If you need more, everything you have to do is open the _Interpreter.Operators.cs_ file, and derive from the corresponding `Node` class :
- `ZeroNode` that takes 0 argument.
- `UnaryNode` that takes 1 argument.
- `BinaryNode` that takes 2 arguments.

Then add the `OperatorAttribute` and fill the symbol and priority.

> INFO: The `OperatorAttribute` is stackable.

> INFO: Lowest priorities are executed first. Default priority is 2 (functions).
 
The following example show the implementation of an operator that halves an operand (unary operator). Its symbols will be `#` and `half`.

```csharp
[Operator("#", 2)]
[Operator("half", 2)]
private class HalfNode : UnaryNode
{
    public HalfNode(Node input)
        : base(input)
    {
    }
    
    public override double GetValue(Interpreter interpreter)
    {
        return this.input.GetValue(interpreter) * .5d;
    }
}
```

The recompile the DLL, and it's done!

```csharp
Interpreter interpreter = new Interpreter();
double a = interpreter.Calculate("#10"); // -> 5
double b = interpreter.Calculate("half(10)"); // -> 5
```

**DO** make a pull request if you want. More operators makes the interpreter better :smile:

## License

This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/fsegaud/Hef.Math.Interpreter/blob/master/LICENSE.md) file for details

## Annex - Handled Operations

| Category           | Function           | Symbol             | Operation                    | Comment                                         |
|--------------------|:------------------:|:------------------:|------------------------------|-------------------------------------------------|
| BASIC              | `sign`             | `±`                | Sign Change                  | -1 should be written ±1 (atl+0177)             |
|                    | `add`              | `+`                | Addition                     |                                                 |
|                    | `sub`              | `-`                | Subtraction                  |                                                 |
|                    | `mult`             | `*`                | Product                      |                                                 |
|                    | `div`              | `/`                | Division                     |                                                 |
|                    | `mod`              | `%`                | Modulo                       |                                                 |
| ADVACED            | `pow`              | `^`                | Power                        |                                                 |
|                    | `sqrt`             |                    | Square Root                  |                                                 |
|                    | `abs`              |                    | Absolute Value               |                                                 |
|                    | `round`            |                    | Round To Integer             |                                                 |
|                    | `min`              |                    | Minimum                      |                                                 |
|                    | `max`              |                    | Maximum                      |                                                 |
|                    | `ceil`             |                    | Ceil To Upper Integer        |                                                 |
|                    | `floor`            |                    | Floor To Lower Integer       |                                                 |
|                    | `trunc`            |                    | Truncate Decimal Part        |                                                 |
|                    | `log`              |                    | Logarithm                    |                                                 |
|                    | `log10`            |                    | Logaritme Base 10            |                                                 |
|                    | `exp`              | `e`                | Exponential                  |                                                 |
| COMPARISON         | `eq`               | `==`               | Equal                        | Returns 1 if true, 0 otherwise                  |
|                    | `ne`               | `!=`               | Not Equal                    | Returns 1 if true, 0 otherwise                  |
|                    | `gt`               | `>`                | Greater Than                 | Returns 1 if true, 0 otherwise                  |
|                    | `gte`              | `>=`               | Greater Or Equal             | Returns 1 if true, 0 otherwise                  |
|                    | `lt`               | `<`                | Less Than                    | Returns 1 if true, 0 otherwise                  |
|                    | `lte`              | `<=`               | Less Or Equal                | Returns 1 if true, 0 otherwise                  |
| LOGICAL            |                    | `!`                | Not                          | `!true` => 0, `!false` => 1                     |
|                    | `and`              | `&&`               | And                          | `true && true` => 1, `true && false` => 0       |
|                    | `or`               | `\|\|`             | Or                           | `true \|\| false` => 1, `false \|\| false` => 0 |
| TRIGONOMETRY       | `cos`              |                    | Cosine                       |                                                 |
|                    | `sin`              |                    | Sine                         |                                                 |
|                    | `tan`              |                    | Tangent                      |                                                 |
|                    | `acos`             |                    | Arccosine                    |                                                 |
|                    | `asin`             |                    | Arcsine                      |                                                 |
|                    | `atan`             |                    | Arctangent                   |                                                 |
|                    | `cosh`             |                    | Hyperbolic Cosine            |                                                 |
|                    | `sinh`             |                    | Hyperbolic Sine              |                                                 |
|                    | `tanh`             |                    | Hyperbolic Tangent           |                                                 |
|                    | `deg2rad`          |                    | Converts degrees to radians  |                                                 |
|                    | `rad2deg`          |                    | Converts radians to degrees  |                                                 |
| RANDOMIZATION      | `rand`             |                    | Random                       | `random(a, b)` => [a, b]                        |
|                    | `dice`             | `d` or `D`         | Dice                         | `dice(a, b)` or `a D b` => [a, a * b]           |
| CONSTANTS          | `pi`               |                    | Value of PI                  | 3.14159...                                      |
|                    | `true`             |                    | True                         | 1                                               |
|                    | `false`            |                    | False                        | 0                                               |
