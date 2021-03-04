### Dependencies

- .NET Framework 4.7.2
- Newtonsoft.Json

# Language
![](https://github.com/AntonShvets0/Tengri/blob/master/assets/logo.png)


### How to use
`$ tengri <path to project> <print statistics? (false/true)>`

### Variable declaration

To declare a variable, you need to write the "var" keyword. Further, when working with a declared variable, you do not need to write it.
<pre>
var fooBar = "data"
fooBar = 5
</pre>

#### Arrays
Arrays are like PHP

<pre>
	var array = [1204, 1368, 1380, 1480]
	var object = [ rulers: [
		[
			name: "Temuchin",
			father:  "Yesügei"
		],
		[
			name: "Ögödei",
			father: "Temuchin"
		]
	] ]
	var mixed = [1204, tumens: [
		[
			id: 1,
			peoples: 9986,
			horses: 29958
		],
		[
			id: 2,
			peoples: 11000,
			horses: 40000
		]
	]]
	</pre>
	
#### Classes, fields, methods
Class declaration is like variable assignment. Var is not necessary..

<pre>
	government = {
		name: "Mongol Khaganate",
		
		@private
		ruler: "Temuchin",
		
		setRuler(name): {
			ruler = name
		}
		
		getName(): {
			return ruler
		}
	}
</pre>

The init method is considered the constructor
<pre>
	people = {
		_name: "",
		
		init(name): {
			_name = name
		}
	}
	
	program = {
		static {
			main(): {
				var peopleClass = people.init("Temuchin")
			}
		}
	}
</pre>


Static methods or fields should be placed in a static block
<pre>
	fooClass = {
		static {
			staticMethod(): {
				// ...
			}
		}
	}
</pre>

Fields or methods that start with _ are automatically considered private. Fields or methods that begin with protected are automatically considered protected.
If you need to move away from the language codestyle and change the visibility of a field or method, then use the attributes

<pre>
	foo = {
		_privateField: "This is private field",
		publicField: "This is public field",
		protectedField: "This is protected field",
		
		@private
		privateMethod(): {
			return "This is private method"
		}
	}
</pre>


#### Import
To access the classes of another file, you should use the import:
<pre>
	import("example.tengri")
	program = {
		static {
			main(): {
				var exampleClass = example.init()
			}
		}
	}
</pre>

#### Conditions
The terms are similar to any other terms in C-like languages. Brackets can be omitted. Else if replaced to "Elif"
<pre>
	if cond {
	
	} elif cond {
	
	} else {
	
	}
</pre>

#### Cycles
##### Foreach
<pre>
	array = [name: "Test", age: 18]
	array {
		console.print(it.Key + " : " + it.Value)
	}
	
	array:item {
		console.print(item.Key + " : " + item.Value)
	}
</pre>

##### For
<pre>
	for i in 0...array.length {
		
	}
</pre>

##### While
<pre>
	while cond {
	
	}
</pre>

##### Do while
<pre>
	do cond {
	
	}
</pre>