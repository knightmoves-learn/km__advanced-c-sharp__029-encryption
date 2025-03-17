# 025 Role Based Authentication

## Lecture

[![# Role Based Authentication](https://img.youtube.com/vi/RTUTtORZ8nM/0.jpg)](https://www.youtube.com/watch?v=RTUTtORZ8nM)

## Instructions

The file `HomeEnergyApi/secrets.json` has been pre-filled with AES:Key and AES:InitializationVector properties. Do NOT change this file.

Also, the `UserDto` and `User` models have each been given a `HomeStreetAddress` and `EncryptedHomeStreetAddress` property respectively, you will be responsible for adding working encryption on these fields in this lesson.

In `HomeEnergyApi/Security/ValueEncryptor.cs`
- Create a public class `ValueEncryptor` with the following private static property names / types
    - key / `string`
    - iv / `string`
- Pass an `IConfiguration` object to this class' constructor, and assign the newly created "key" and "iv" properties to the result of reading the "AES:Key" and "AES:InitializationVector" properties from the passed `IConfiguration`
- Create a public method `Encrypt()` returning a `string`
    - `Encrypt()` should take one argument of type `string`
    - `Encrypt()` should return a new `ArgumentException` with the value "Key must be 32 bytes and IV must be 16 bytes long." when the "key" property is not 32 characters in length, or the "iv" is not 16 characters in length
    - `Encrypt()` should create a new variable representing an AES Algorithm from the result of `Aes.Create()`, using the `using` keyword as to ensure it is properly disposed. Then, set the `Key` and `IV` properties on the newly created variable to the `byte []` value of the class level "key" and "iv" properties
    - `Encrypt()` should create a new variable from the result of calling `CreateEncryptor` on the newly created AES Algorithm variable, passing it the `Key` and `IV` properties from the newly created AES Algorithm variable
    - `Encrypt()` should initialize a variable of type `byte []`
    - `Encrypt()` should create a new variable of type `MemoryStream`, using the `using` keyword as to ensure it is properly disposed.
    - `Encrypt()` should create a new variable of type `CryptoStream` passing the newly created `MemoryStream`, empty `byte []`, and `CryptoStreamMode.Write` into it's constructor, using the `using` keyword as to ensure it is properly disposed.
    - `Encrypt()` should create a new variable of type `StreamWriter` passing the newly created `CryptoStream` into it's constructor, using the `using` keyword as to ensure it is properly disposed. Then, use the `Write()` method on the `StreamWriter` passing the method's `string` argument. Then, set the initialized `byte []` to the value of calling `ToArray()` on the newly created `MemortyStream` inside of its `using` block
    - `Encrypt()` should return the result of calling `Convert.ToBase64String` passing the `byte []` variable
- Create a public method `Decrypt()` returning a `string`
    - `Decrypt()` should take one argument of type `string`
    - `Decrypt()` should return a new `ArgumentException` with the value "Key must be 32 bytes and IV must be 16 bytes long." when the "key" property is not 32 characters in length, or the "iv" is not 16 characters in length
    - `Decrypt()` should create a new variable representing an AES Algorithm from the result of `Aes.Create()`, using the `using` keyword as to ensure it is properly disposed. Then, set the `Key` and `IV` properties on the newly created variable to the `byte []` value of the class level "key" and "iv" properties
    - `Decrypt()` should create a new variable from the result of calling `CreateDecryptor` on the newly created AES Algorithm variable, passing it the `Key` and `IV` properties from the newly created AES Algorithm variable
    - `Decrypt()` should initialize a variable of type `string`
    - `Decrypt()` should create a new `byte []` from the result of calling `Convert.FromBase64String()` passing the method's `string` argument
    - `Decrypt()` should create a new variable of type `MemoryStream` passing the newly created `byte []`, using the `using` keyword as to ensure it is properly disposed.
    - `Decrypt()` should create a new variable of type `CryptoStream` passing the newly created `MemoryStream`, the variable holding the AES Algorithm Decryptor, and `CryptoStreamMode.Read` into it's constructor, using the `using` keyword as to ensure it is properly disposed.
    - `Decrypt()` should create a new variable of type `StreamWriter` passing the newly created `CryptoStream` into it's constructor, using the `using` keyword as to ensure it is properly disposed. Then, assign the value of calling `ReadToEnd()` method on the `StreamWriter` to the intialized `string`. 
    - `Decrypt()` should return the `string` that the `StreamReader` set the value on

In `HomeEnergyApi/Controllers/AuthenticationController.cs`
- Add a private readonly property to `AuthenticationController` of type `ValueEncryptor`, also adding this property to the class' constructor
- Refactor the `Register()` method to...
    - Set the `EncryptedHomeStreetAddress` property on the user being registered to the result of passing the `HomeStreetAddress` property to the `Encrypt()` method on the `ValueEncryptor` property
- You may want to add additional logging similar to what was shown in the lecture to the `Register()` and `Token()` methods to see the encrypted and decrypted street address value as a user is registered and given a token. This will display in this assignment's autograding output. However, this is not necessary to complete the lesson.

In `HomeEnergyApi/Program.cs`
- Add a singleton service to the builder of type `ValueEncryptor`

In your terminal
- ONLY IF you are working on codespaces or a different computer/environment as the previous lesson and don't have `dotnet-ef` installed globally, run `dotnet tool install --global dotnet-ef`, otherwise skip this step
    - To check if you have `dotnet-ef` installed, run `dotnet-ef --version`
- Run `dotnet ef migrations add AddUserStreetAddressEncryption`
- Run `dotnet ef database update`

## Additional Information
- Do not remove or modify anything in `HomeEnergyApi.Tests`
- Some Models may have changed for this lesson from the last, as always all code in the lesson repository is available to view
- Along with `using` statements being added, any packages needed for the assignment have been pre-installed for you, however in the future you may need to add these yourself

## Building toward CSTA Standards:
- Give examples to illustrate how sensitive data can be affected by malware and other attacks (3A-NI-05) https://www.csteachers.org/page/standards
- Recommend security measures to address various scenarios based on factors such as efficiency, feasibility, and ethical impacts (3A-NI-06) https://www.csteachers.org/page/standards
- Compare various security measures, considering tradeoffs between the usability and security of a computing system (3A-NI-07) https://www.csteachers.org/page/standards
- Explain tradeoffs when selecting and implementing cybersecurity recommendations (3A-NI-08) https://www.csteachers.org/page/standards
- Compare ways software developers protect devices and information from unauthorized access (3B-NI-04) https://www.csteachers.org/page/standards
- Explain security issues that might lead to compromised computer programs (3B-AP-18) https://www.csteachers.org/page/standards

## Resources
- https://en.wikipedia.org/wiki/Cryptographic_hash_function

Copyright &copy; 2025 Knight Moves. All Rights Reserved.
