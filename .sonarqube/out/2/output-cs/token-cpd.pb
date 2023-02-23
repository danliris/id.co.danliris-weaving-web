¥
{D:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Infrastructure.External.DanLirisClient.CoreMicroservice\CoreClient.cs
	namespace 	
Infrastructure
 
. 
External !
.! "
DanLirisClient" 0
.0 1
CoreMicroservice1 A
{ 
public 

class 

CoreClient 
: 
ICoreClient )
{		 
private

 
readonly

 

HttpClient

 #
_http

$ )
;

) *
private 
readonly 
MasterDataSettings +
	_settings, 5
;5 6
public 

CoreClient 
( 

HttpClient $
http% )
,) *
MasterDataSettings+ =
settings> F
)F G
{ 	
_http 
= 
http 
; 
	_settings 
= 
settings  
;  !
} 	
public 
Task 
< 
dynamic 
> #
RetrieveUnitDepartments 4
(4 5
)5 6
{ 	
throw 
new 
System 
. #
NotImplementedException 4
(4 5
)5 6
;6 7
} 	
	protected 
async 
Task 
< 
string #
># $
GetTokenAsync% 2
(2 3
)3 4
{ 	
var 
response 
= 
_http  
.  !
	PostAsync! *
(* +
	_settings+ 4
.4 5
TokenEndpoint5 B
,B C
new 
StringContent !
(! "
JsonConvert" -
.- .
SerializeObject. =
(= >
new> A
{B C
usernameD L
=M N
	_settingsO X
.X Y
UsernameY a
,a b
passwordc k
=l m
	_settingsn w
.w x
Password	x Ä
}
Å Ç
)
Ç É
,
É Ñ
Encoding
Ö ç
.
ç é
UTF8
é í
,
í ì
$str
î ¶
)
¶ ß
)
ß ®
;
® ©
dynamic 
tokenResult 
=  !
new" %
{& '
}( )
;) *
if 
( 
response 
. #
IsCompletedSuccessfully 0
)0 1
{   
tokenResult!! 
=!! 
JsonConvert!! )
.!!) *
DeserializeObject!!* ;
<!!; <
dynamic!!< C
>!!C D
(!!D E
await!!E J
response!!K S
.!!S T
Result!!T Z
.!!Z [
Content!![ b
.!!b c
ReadAsStringAsync!!c t
(!!t u
)!!u v
)!!v w
;!!w x
}"" 
return$$ 
tokenResult$$ 
.$$ 
accessToken$$ *
;$$* +
}%% 	
}&& 
}'' ø
|D:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Infrastructure.External.DanLirisClient.CoreMicroservice\ICoreClient.cs
	namespace 	
Infrastructure
 
. 
External !
.! "
DanLirisClient" 0
.0 1
CoreMicroservice1 A
{ 
public 

	interface 
ICoreClient  
{ 
Task 
< 
dynamic 
> #
RetrieveUnitDepartments -
(- .
). /
;/ 0
} 
}		 ®
ÉD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Infrastructure.External.DanLirisClient.CoreMicroservice\MasterDataSettings.cs
	namespace 	
Infrastructure
 
. 
External !
.! "
DanLirisClient" 0
.0 1
CoreMicroservice1 A
{ 
public 

class 
MasterDataSettings #
{ 
public 
string 
Endpoint 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
string 
TokenEndpoint #
{$ %
get& )
;) *
set+ .
;. /
}0 1
public		 
string		 
Password		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		+ ,
public 
string 
Username 
{  
get! $
;$ %
set& )
;) *
}+ ,
} 
} 