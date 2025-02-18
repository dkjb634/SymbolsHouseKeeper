Usage:
The script takes the specified path and process directories in it one by one.
For every directory it found (such a directory is supposed to be a "ModuleName.pdb", it looks at its subdirectories (that are supposed to be guids of .pdb).
Script finds the directory with the most recent creation timestamp and deletes the rest folders.

Sample: "C:\ThePath\SymbolsCache" is specified.
Symbols cache has that structure:
```
SymbolsCache/
--|__Module1.pdb/
     |__guidold1/
     |__guidold2/
     |__themostrecentguid/
--|__Module2.pdb/
     |__guidold1/
     |__guidold2/
     |__themostrecentguid/
```
As a result of script execution, the result will be:
```
SymbolsCache/
--|__Module1.pdb/
     |__themostrecentguid/
--|__Module2.pdb/
     |__themostrecentguid/
```