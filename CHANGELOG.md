# CHANGELOG

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

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
