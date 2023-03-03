þ
bD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Infrastructure\Content\IExtensionMetadata.cs
	namespace 	
Infrastructure
 
{ 
public 

	interface 
IExtensionMetadata '
{		 
IEnumerable

 
<

 

StyleSheet

 
>

 
StyleSheets

  +
{

, -
get

. 1
;

1 2
}

3 4
IEnumerable 
< 
Script 
> 
Scripts #
{$ %
get& )
;) *
}+ ,
IEnumerable 
< 
MenuItem 
> 
	MenuItems '
{( )
get* -
;- .
}/ 0
} 
} ­	
XD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Infrastructure\Content\MenuItem.cs
	namespace 	
Infrastructure
 
{ 
public 

class 
MenuItem 
{ 
public 
string 
Url 
{ 
get 
;  
set! $
;$ %
}& '
public		 
string		 
Name		 
{		 
get		  
;		  !
}		" #
public

 
int

 
Position

 
{

 
get

 !
;

! "
}

# $
public 
MenuItem 
( 
string 
url "
," #
string$ *
name+ /
,/ 0
int1 4
position5 =
)= >
{ 	
this 
. 
Url 
= 
url 
; 
this 
. 
Name 
= 
name 
; 
this 
. 
Position 
= 
position $
;$ %
} 	
} 
} —
VD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Infrastructure\Content\Script.cs
	namespace 	
Infrastructure
 
{ 
public 

class 
Script 
{ 
public 
string 
Url 
{ 
get 
;  
set! $
;$ %
}& '
public		 
int		 
Position		 
{		 
get		 !
;		! "
}		# $
public 
Script 
( 
string 
url  
,  !
int" %
position& .
). /
{ 	
this 
. 
Url 
= 
url 
; 
this 
. 
Position 
= 
position $
;$ %
} 	
} 
} £
ZD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Infrastructure\Content\StyleSheet.cs
	namespace 	
Infrastructure
 
{ 
public 

class 

StyleSheet 
{ 
public 
string 
Url 
{ 
get 
;  
set! $
;$ %
}& '
public		 
int		 
Position		 
{		 
get		 !
;		! "
}		# $
public 

StyleSheet 
( 
string  
url! $
,$ %
int& )
position* 2
)2 3
{ 	
this 
. 
Url 
= 
url 
; 
this 
. 
Position 
= 
position $
;$ %
} 	
} 
} ›
VD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Infrastructure\IWebApiContext.cs
	namespace 	
Infrastructure
 
{ 
public 

	interface 
IWebApiContext #
:$ %
IWorkContext& 2
{ 
string 

ApiVersion 
{ 
get 
;  
}! "
} 
}		 