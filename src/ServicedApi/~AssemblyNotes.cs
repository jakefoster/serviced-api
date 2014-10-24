/*
NAMING THOUGHTS

Universal Scope [Thread-common]
-----------------------------------
We don't currently have a need for this but it might be:
- AppContext
- ApiContext
- DomainContext

Request Scope [Thread-specific]
-----------------------------------
ExecutionContext - has request scope - UPDATE: So now this is actually defined down in the API assembly, 
    along with all the security stuff, for complicated reasons having to do with interfaces not being
    able to define internals, etc.  I really don't want to get into this except to say that it works, is
    fakeable for testing, is stable, and is relatively easy to use.  JF
ExecutionContext.Current.User is IServiceUser

Operation Scope [Thread-specific]
-----------------------------------
ServiceContext - has operation scope
ServiceContext.Current.DataContext is IDataContext
*/