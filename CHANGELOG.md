# CHANGELOG

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [1.0.0] 2017.09.??

### Added

- Added binary operators `<<`, `>>`, `&` and `|`.
- Added comparison operator `!=` (with alias `ne`).
- Added new operators `floor`, `ceil`, `trunc`.
- Added new operators `e`, `log`, `log10`.
- Added aliases to existing operators `<`, `<=`, `>` and `>=`.

### Changed

- Operators priority is now from lowest (0) to highest (_int.Max_);
- Formula syntax: Alphanumeric operators should now be separated from operants by a blanck space or brackets (e.g.: `e2` should be written `e 2` or `e(2)`).

### Removed

- Removed some deprecated operation aliases (`degrad` and `raddeg`).
- Removed some deprecated functions related to unnamed contexts :
  - Constructor `Interpreter(IInterpreterContext)`.
  - Function `SetContext(IInterpreterContext)`.

### Fixed

- Operators that contain digits in their names no longer fail (e.g.: `deg2rad`).
- Fixed an issue related to mixed operators (i.e.: `!` was mixed to `!=`).

## [0.2.0-alpha] - 2017.08.29

### Added

- The interpreter now handles the use of multiple contexts (identified by name).
- Some operators now accept a function-style writer (eg. `min(1, 2)` instead of `min 1 2`.
- Copies of all previously prcessed formulae are now kept in cache (optimization).

### Changed
- Operations and values are now stored a tree (optimization).

### Fixed

- Fixed precedence issues related to `|` and `&` boolean operations.
2`).

### Deprecated

- Constructor `Interpreter(IInterpreterContext)` is now deprecated. Use `Interpreter.SetContext(string, IINterpreterContext)` instead.
- Function `SetContext(IInterpreterContext)` is now deprecated. Use `Interpreter.SetContext(string, IINterpreterContext)` instead.

## [0.1.1-alpha] - 2017.08.22

### Added

- Added new operations : `tan`, `cosh`, `sinh`, `tanh`, `acos`, `asin`, `atan`, `deg2rad`, `rad2deg`, `and`, `or`, `&`, `|`
- Added new constants : `true`, `false`.

## [0.1.1-alpha] - 2017.08.20
