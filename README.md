# Hef.Math.Interpreter

This interpreter takes a math formula as input, breaks it into a chain of values and operations, and outputs a numeric result.
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

You can also install the correpsonding _NuGet_ package at [http://nuget.org/Hef.Math.Interpreter](http://nuget.org/Hef.Math.Interpreter)

Or install it using the _NuGet_ console.

```
Install-Package Hef.Math.Interperter
```

### Examples

The interpreter accepts multiple formula coding conventions. You can uses spaces, or not. You can use parenthesis, or not. It's up to you.

Here is the simplest exemple.

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

The following example highlights the use of `Hef.Math.IInterpreterContext`, that allow the interpreter to access variables provided by other obejects.

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

## Contributing

### How To Define Additional Operations

==`// TODO`==

## License

This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/fsegaud/Hef.Math.Interpreter/blob/master/LICENSE.md) file for details