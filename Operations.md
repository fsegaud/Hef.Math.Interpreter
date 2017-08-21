# List Of Handled Operations

### Basic Operators
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

### Advanced Operators

| Symbol  | Operation      | Comment | Version |
|:-------:|----------------|---------|:-------:|
| `abs`   | Absolute Value |         | 0.1.0   |
| `round` | Round          |         | 0.1.0   |
| `min`   | Minimum        |         | 0.1.0   |
| `max`   | Maximum        |         | 0.1.0   |

### Comparison Operators

| Symbol       | Operation        | Comment                    | Version |
|:------------:|------------------|----------------------------|:-------:|
| `==` or `eq` | Equal            | 1 == 1 -> 1, 1 eq 2 -> 0   | 0.1.0   |
| `gt`         | Greater Than     | 1 gt 1 -> 0, 1 gt 2 -> 1   | 0.1.0   |
| `gte`        | Greater Or Equal | 1 gte 0 -> 0, 1 gte 1 -> 1 | 0.1.0   |
| `lt`         | Less Than        | 1 lt 1 -> 0, 1 lt 2 -> 1   | 0.1.0   |
| `lte`        | Less Or Equal    | 1 lte 1 -> 1, 1 lte 0 -> 0 | 0.1.0   |

### Logical Operators

| Symbol       | Operation        | Comment                    | Version |
|:------------:|------------------|----------------------------|:-------:|
| `!`          | Not              | !0 -> 1, !1 -> 0           | 0.1.0   |
| `&` or `and` | And              | true & true -> true        | 0.1.1   |
| `\|` or `or` | Or               | true & false -> true       | 0.1.1   |

### Trigonometry

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

### Randomization

| Symbol     | Operation | Comment         | Version |
|:----------:|-----------|-----------------|:-------:|
| `rand`     | Random    | rand 5 -> [0,5] | 0.1.0   |
| `d` or `D` | Dice      | 2d6 -> [2,12]   | 0.1.0   |

### Constants

| Symbol  | Operation     | Comment | Version |
|:-------:|---------------|---------|:-------:|
| `pi`    | Value of PI   | 3.14... | 0.1.0   |
| `true`  | Boolean true  | 1       | 0.1.1   |
| `false` | Boolean false | 0       | 0.1.1   |
