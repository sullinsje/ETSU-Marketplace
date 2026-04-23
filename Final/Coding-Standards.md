# Coding Standards

## Naming Conventions
- Give variables and methods self descriptive names. This will reduce the need to provide detailed comments in code. Code should be easy to read and understand.

## Commenting
- As mentioned previously, self-descriptive code over detailed comments. Put comments when necessary or for leaving notes for later.
- An area where comments may be useful is a class summary in important classes. Examples are Controllers and Repositories which provide the core functionality of the application

## Modularity
- We are using C#, an object-oriented language. Utilize its ability for containing smaller functions in logically similar classes.
- This will primarily be in our Services folder where the business logic resides.
- Remember the pillars of OOP: **Abstraction**, **Encapsulation**, **Inheritance**, and **Polymorphism**
  - The Repository Layer is utilizing a **Generic Repository Pattern**; inheritance and polymorphism are key here to encourage DRY code

## Formatting
- Use the format shortcuts for the programming language's VSCode Formatter extensions. This will work for keeping clean indents and whitespace in code.

## Checks in code
- Make sure to provide checks in your code to prevent unexpected errors; check for sign-in status, ensure variables aren't null, etc.
