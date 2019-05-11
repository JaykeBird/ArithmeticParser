# Arithmetic-Parser-made-easy

A simple parser/evaluator for math expressions in C#.

## How to use
`double result = Parser.Evaluate("(3+4)*-2");`

If the expression cannot be evaluated (such as `Parser.Evaluate("apple");`, it will throw a FormatException).

## Supported features
 - Addition, subtraction, multiplication, division
 - Exponents (`2^3`)
 - Decimals (`2*(0.3)` and `2*(0,3)` (depends upon culture, see below))
 - Implied multiplication (`(2-3)(4/6)` and `3(4+6)`)
 - Negation (`-(4+5)`)

### Note about decimal separators
Some cultures use . for decimal separators (0.5) while others use , (0,5). Due to how C# works, the decimal point used depends upon the culture the current thread is running on. If you want it one way or the other, you'll want to change the [UI culture](https://stackoverflow.com/questions/7000509/how-to-change-currentculture-at-runtime).

[Learn which cultures use which symbol](https://en.wikipedia.org/wiki/Decimal_separator#Arabic_numerals)

## License
This code is released under [the MIT License](LICENSE).
