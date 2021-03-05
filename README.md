### Dependencies

- .NET Framework 4.7.2
- Newtonsoft.Json

# Language
![](https://github.com/AntonShvets0/Tengri/blob/master/assets/logo.png)
# Tengri 2.0


### How to use
`$ tengri <path to project> <print statistics? (false/true)>`

### Variable declaration

To declare a variable, you need to write the "var" keyword. Further, when working with a declared variable, you do not need to write it.
<pre>
var fooBar = "data"
fooBar = 5
</pre>

And in language exists global variables:

<pre>
variable = "Test"
program = {
    static {
        main: () {
            console.print(global.variable) // Test
        }
    }
}

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
    
    setRuler: (name) {
        ruler = name
    }
    
    getName: () {
        return ruler
    }
}
</pre>

The init method is considered the constructor
<pre>
people = {
    _name: "",
    
    init: (name) {
        _name = name
    }
}

program = {
    static {
        main: () {
            var peopleClass = people.init("Temuchin")
        }
    }
}
</pre>


Static methods or fields should be placed in a static block
<pre>
fooClass = {
    static {
        staticMethod: () {
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
    privateMethod: () {
        return "This is private method"
    }
}
</pre>

And in language exists procedure style for functions:
<pre>
fun test() {
    console.print("Hi!")
}


program = {
    static {
       main: () {
            global.test()
       }
    }
}

</pre>


#### Import and export
To access the classes of another file, you should use the import. Like a require from Node.Js:

program.tengri:
<pre>
program = {
    static {
        main(): {
            var example = import "example"
            example["function"]() // print hello world
        }
    }
}
</pre>

example.tengri
<pre>
// another code...

export [
    function: () {
        console.print("Hello world")
    }
]
</pre>


#### Conditions
The terms are similar to any other terms in C-like languages. Brackets can be omitted. Else if replaced to "Elif"
<pre>
if cond {

} elif cond {

} else {

}
</pre>

#### Loops
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

### Exceptions
To throw exception, use superglobal function throw():
<pre>
throw(exception.init("Message"))
</pre>

You can catch it:
<pre>
try {
    throw(exception.init("Message")) 
}

// or
try {
    throw(exception.init("Message")) 
} catch {
    console.print(ex.message)
}

// or
try {
    throw(exception.init("Message")) 
} catch:exception {
    console.print(exception.message)
}

// or
try {
    throw(exception.init("Message")) 
} catch:exception {
    console.print(exception.message)
} finally {
    console.print("For Tengri!")
}

</pre>

### Threads
<pre>
var thread = thread.init(() {
console.print("thread func")
})

thread.start()
thread.stop()

</pre>