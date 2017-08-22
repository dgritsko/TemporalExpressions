```
<expression>    ->  { <expr_body> }
<expr_body>     ->  <identifier>(<arguments>)     // is it possible to have an expression without arguments? if so, use "<identifier> | <identifier> : <arguments> " here instead
<identifier>    -> [a-z]+
<arguments>     -> <argument>
                 | <argument>,<arguments>
<argument>      -> <scalar>
                 | <expression>
<scalar>        -> [0-9]+

ID: DayInMonth
Fields: Count (int), Day (int)
Example:    {DayInMonth(0,0)}

ID: Difference
Fields: Included (expression), Excluded (expression)
Example:    {Difference({},{})}

ID: Intersection
Fields: Elements (list of expressions)
Example:    {Intersection({},{})}

ID: RangeEachYear
Fields: StartMonth (int), EndMonth (int), StartDay (int), EndDay (int)
Example: {RangeEachYear(0,0,0,0)}

ID: Union
Fields: Elements (list of expressions)
Example: {Union({},{},{})}
```
