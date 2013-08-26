# Placeholders.js Monkey Patch #

This provides supplementary features that are not supported by [Placeholders.js](http://jamesallardice.github.io/Placeholders.js), especially for IE8 and IE9.


# Issues #

On both IE8 and IE9, the placeholder polyfills don't work properly in certain conditions. This monkey patch is to help workout for those conditions.


# Getting Started #

## Prerequisites ##

This monkey patch requires [jQuery](http://jquery.com). It's been tested on 1.10.2.


## Installation ##

Put the following CSS code after your main CSS.

    <!--[if lte IE 9]>
      <link type="text/css" rel="stylesheet" href="css/jquery.Placeholders.monkey.patch.min.css" />
    <![endif]-->

And put the following JS code after `jQuery`.

    <!--[if lte IE 9]>
      <script type="text/javascript" src="js/jquery.Placeholders.monkey.patch.min.js"></script>
      <script type="text/javascript">
        Placeholders.For = {
          "inputs": ["input:text", "textarea"],
          "fakePasswords": ["#fake-password"],
          "forms": ["form"]
        };
      </script>
    <![endif]-->

What `Placeholders.For` does is that you can specifies particular range of input fields, password fields and forms to apply this monkey patch. More details can be found in the `example` directory.

That's it for the installation.


## How to Use ##

There are four main functions &ndash; `addPlaceholderAttributes`, `removePlaceholderAttributes`, `applyPlaceholderAttributes`, and `removePlaceholderValue`.


### applyPlaceholderAttributes ###

This adds or removes custom attributes manipulated from `Placeholders.js` on a particular event.


### addPlaceholderAttributes ###

This adds custom attributes manipulated from `Placeholders.js` to an input element, on a particular event.


### removePlaceholderAttributes ###

This removes custom attributes manipulated from `Placeholders.js` from an input element, on a particular event.


### removePlaceholderValue ###

This removes placeholder value from an input element value, on a particular event.

More details can be found in the `example` directory.


# Acknowledgment #

This is not a perfect solution to provide additional improvement for `Placeholders.js`. Any contribution is welcome!


# License #

**Placeholders.js Monkey Patch** is released under [MIT License](http://opensource.org/licenses/MIT).

> The MIT License (MIT)
> 
> Copyright (c) 2013 [aliencube.org](http://aliencube.org)
> 
> Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
> furnished to do so, subject to the following conditions:
> 
> The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
> 
> THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
