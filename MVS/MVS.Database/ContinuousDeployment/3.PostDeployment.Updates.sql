﻿/*
This will be executed during the post-deployment phase.
Use it to apply scripts which are supposed to modify the 
data after the schema update took place.

!!!Make sure your scripts are idempotent(repeatable)!!!

Example invocation:
EXEC sp_execute_script @sql = 'UPDATE Table....', @author = 'Your Name'
*/
/*
EXEC sp_execute_script @sql = '
UPDATE bdo.ContactType
SET IsManager = 1
WHERE Name LIKE ''%manager%''
', @author = 'Radoslav Gatev'
*/