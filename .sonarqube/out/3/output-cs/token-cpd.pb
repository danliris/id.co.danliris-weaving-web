¸
VD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Barebone\Actions\UseMvcAction.cs
	namespace		 	
Barebone		
 
.		 
Actions		 
{

 
public 

class 
UseMvcAction 
: 
IUseMvcAction  -
{ 
public 
int 
Priority 
=> 
$num #
;# $
public 
void 
Execute 
( 
IRouteBuilder )
routeBuilder* 6
,6 7
IServiceProvider8 H
serviceProviderI X
)X Y
{ 	
routeBuilder 
. 
MapRoute !
(! "
name" &
:& '
$str( 1
,1 2
template3 ;
:; <
$str= T
,T U
defaultsV ^
:^ _
new` c
{d e

controllerf p
=q r
$strs }
,} ~
action	 Ö
=
Ü á
$str
à è
}
ê ë
)
ë í
;
í ì
} 	
} 
} “
`D:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Barebone\Controllers\BareboneController.cs
	namespace 	
Barebone
 
. 
Controllers 
{		 
public

 

class

 
BareboneController

 #
:

$ %
ControllerBase

& 4
{ 
public 
BareboneController !
(! "
IStorage" *
storage+ 2
)2 3
:4 5
base6 :
(: ;
storage; B
)B C
{ 	
} 	
public 
ActionResult 
Index !
(! "
)" #
{ 	
return 
this 
. 
View 
( 
new  !
IndexViewModelFactory! 6
(6 7
)7 8
.8 9
Create9 ?
(? @
)@ A
)A B
;B C
} 	
} 
} ñ
\D:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Barebone\Controllers\ControllerBase.cs
	namespace 	
Barebone
 
. 
Controllers 
{ 
public 

abstract 
class 
ControllerBase (
:) *
	Microsoft+ 4
.4 5

AspNetCore5 ?
.? @
Mvc@ C
.C D

ControllerD N
{ 
	protected 
IStorage 
Storage "
{# $
get% (
;( )
private* 1
set2 5
;5 6
}7 8
public 
ControllerBase 
( 
IStorage &
storage' .
). /
{ 	
this 
. 
Storage 
= 
storage "
;" #
} 	
} 
[ 
ApiController 
] 
public 

abstract 
class 
ControllerApiBase +
:, -
	Microsoft. 7
.7 8

AspNetCore8 B
.B C
MvcC F
.F G
ControllerBaseG U
{ 
	protected 
IStorage 
Storage "
{# $
get% (
;( )
}* +
	protected 
IWebApiContext  
WorkContext! ,
{- .
get/ 2
;2 3
}4 5
	protected 
	IMediator 
Mediator $
{% &
get' *
;* +
}, -
public 
ControllerApiBase  
(  !
IServiceProvider! 1
serviceProvider2 A
)A B
{   	
this!! 
.!! 
Storage!! 
=!! 
serviceProvider!! *
.!!* +

GetService!!+ 5
<!!5 6
IStorage!!6 >
>!!> ?
(!!? @
)!!@ A
;!!A B
this"" 
."" 
WorkContext"" 
="" 
serviceProvider"" .
."". /

GetService""/ 9
<""9 :
IWebApiContext"": H
>""H I
(""I J
)""J K
;""K L
this## 
.## 
Mediator## 
=## 
serviceProvider## +
.##+ ,

GetService##, 6
<##6 7
	IMediator##7 @
>##@ A
(##A B
)##B C
;##C D
}$$ 	
	protected&& 
IActionResult&& 
Ok&&  "
<&&" #
T&&# $
>&&$ %
(&&% &
T&&& '
data&&( ,
,&&, -
object&&. 4
info&&5 9
=&&: ;
null&&< @
,&&@ A
string&&B H
message&&I P
=&&Q R
null&&S W
)&&W X
{'' 	
return(( 
base(( 
.(( 
Ok(( 
((( 
new(( 
{)) 

apiVersion** 
=** 
this** !
.**! "
WorkContext**" -
.**- .

ApiVersion**. 8
,**8 9
success++ 
=++ 
true++ 
,++ 
data,, 
,,, 
info-- 
,-- 
message.. 
,.. 

statusCode// 
=// 
HttpStatusCode// +
.//+ ,
OK//, .
}00 
)00 
;00 
}11 	
}22 
}33 Œ
SD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Barebone\ExtensionMetadata.cs
	namespace 	
Barebone
 
{ 
public		 

class		 
ExtensionMetadata		 "
:		# $
IExtensionMetadata		% 7
{

 
public 
IEnumerable 
< 

StyleSheet %
>% &
StyleSheets' 2
{ 	
get 
{ 
return 
new 

StyleSheet %
[% &
]& '
{ 
new 

StyleSheet "
(" #
$str# P
,P Q
$numR U
)U V
,V W
new 

StyleSheet "
(" #
$str# 9
,9 :
$num; >
)> ?
} 
; 
} 
} 	
public 
IEnumerable 
< 
Script !
>! "
Scripts# *
{ 	
get 
{ 
return 
new 
Script !
[! "
]" #
{ 
new 
Script 
( 
$str V
,V W
$numX [
)[ \
,\ ]
new 
Script 
( 
$str h
,h i
$numj m
)m n
,n o
new 
Script 
( 
$str	 Å
,
Å Ç
$num
É Ü
)
Ü á
,
á à
new   
Script   
(   
$str   5
,  5 6
$num  7 :
)  : ;
}!! 
;!! 
}"" 
}## 	
public%% 
IEnumerable%% 
<%% 
MenuItem%% #
>%%# $
	MenuItems%%% .
=>%%/ 1
new%%2 5
MenuItem%%6 >
[%%> ?
]%%? @
{%%A B
}%%C D
;%%D E
}&& 
}'' ™
bD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Barebone\ViewComponents\MenuViewComponent.cs
	namespace 	
Barebone
 
. 
ViewComponents !
{		 
public

 

class

 
MenuViewComponent

 "
:

# $
ViewComponent

% 2
{ 
public 
async 
Task 
<  
IViewComponentResult .
>. /
InvokeAsync0 ;
(; <
)< =
{ 	
await 
Task 
. 
Yield 
( 
) 
; 
return 
this 
. 
View 
( 
new   
MenuViewModelFactory! 5
(5 6
)6 7
.7 8
Create8 >
(> ?
)? @
)@ A
;A B
} 	
} 
} ≥
eD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Barebone\ViewComponents\ScriptsViewComponent.cs
	namespace 	
Barebone
 
. 
ViewComponents !
{		 
public

 

class

  
ScriptsViewComponent

 %
:

& '
ViewComponent

( 5
{ 
public 
async 
Task 
<  
IViewComponentResult .
>. /
InvokeAsync0 ;
(; <
)< =
{ 	
await 
Task 
. 
Yield 
( 
) 
; 
return 
this 
. 
View 
( 
new  #
ScriptsViewModelFactory! 8
(8 9
)9 :
.: ;
Create; A
(A B
)B C
)C D
;D E
} 	
} 
} ø
iD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Barebone\ViewComponents\StyleSheetsViewComponent.cs
	namespace 	
Barebone
 
. 
ViewComponents !
{		 
public

 

class

 $
StyleSheetsViewComponent

 )
:

* +
ViewComponent

, 9
{ 
public 
async 
Task 
<  
IViewComponentResult .
>. /
InvokeAsync0 ;
(; <
)< =
{ 	
await 
Task 
. 
Yield 
( 
) 
; 
return 
this 
. 
View 
( 
new  '
StyleSheetsViewModelFactory! <
(< =
)= >
.> ?
Create? E
(E F
)F G
)G H
;H I
} 	
} 
} ÿ
dD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Barebone\ViewModels\Barebone\IndexViewModel.cs
	namespace 	
Barebone
 
. 

ViewModels 
. 
Barebone &
{ 
public 

class 
IndexViewModel 
{ 
} 
}		 Ù
kD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Barebone\ViewModels\Barebone\IndexViewModelFactory.cs
	namespace 	
Barebone
 
. 

ViewModels 
. 
Barebone &
{ 
internal 
class !
IndexViewModelFactory (
{ 
public 
IndexViewModel 
Create $
($ %
)% &
{		 	
return

 
new

 
IndexViewModel

 %
(

% &
)

& '
{ 
} 
; 
} 	
} 
} í
nD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Barebone\ViewModels\Shared\MenuItem\MenuItemViewModel.cs
	namespace 	
Barebone
 
. 

ViewModels 
. 
Shared $
{ 
public 

class 
MenuItemViewModel "
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
;		  !
set		" %
;		% &
}		' (
}

 
} Ï
uD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Barebone\ViewModels\Shared\MenuItem\MenuItemViewModelFactory.cs
	namespace 	
Barebone
 
. 

ViewModels 
. 
Shared $
{ 
public 

class $
MenuItemViewModelFactory )
{		 
public

 
MenuItemViewModel

  
Create

! '
(

' (
MenuItem

( 0
menuItem

1 9
)

9 :
{ 	
return 
new 
MenuItemViewModel (
(( )
)) *
{ 
Url 
= 
menuItem 
. 
Url "
," #
Name 
= 
menuItem 
.  
Name  $
} 
; 
} 	
} 
} ∂
fD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Barebone\ViewModels\Shared\Menu\MenuViewModel.cs
	namespace 	
Barebone
 
. 

ViewModels 
. 
Shared $
{ 
public 

class 
MenuViewModel 
{		 
public

 
IEnumerable

 
<

 
MenuItemViewModel

 ,
>

, -
	MenuItems

. 7
{

8 9
get

: =
;

= >
set

? B
;

B C
}

D E
} 
} ç
mD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Barebone\ViewModels\Shared\Menu\MenuViewModelFactory.cs
	namespace		 	
Barebone		
 
.		 

ViewModels		 
.		 
Shared		 $
{

 
public 

class  
MenuViewModelFactory %
{ 
public 
MenuViewModel 
Create #
(# $
)$ %
{ 	
List 
< 
MenuItem 
> 
	menuItems $
=% &
new' *
List+ /
</ 0
MenuItem0 8
>8 9
(9 :
): ;
;; <
foreach 
( 
IExtensionMetadata '
extensionMetadata( 9
in: <
ExtensionManager= M
.M N
GetInstancesN Z
<Z [
IExtensionMetadata[ m
>m n
(n o
)o p
)p q
	menuItems 
. 
AddRange "
(" #
extensionMetadata# 4
.4 5
	MenuItems5 >
)> ?
;? @
return 
new 
MenuViewModel $
($ %
)% &
{ 
	MenuItems 
= 
	menuItems %
.% &
OrderBy& -
(- .
mi. 0
=>1 3
mi4 6
.6 7
Position7 ?
)? @
.@ A
SelectA G
(G H
mi 
=> 
new $
MenuItemViewModelFactory 2
(2 3
)3 4
.4 5
Create5 ;
(; <
mi< >
)> ?
) 
} 
; 
} 	
} 
} ª
lD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Barebone\ViewModels\Shared\Scripts\ScriptsViewModel.cs
	namespace 	
Barebone
 
. 

ViewModels 
. 
Shared $
{ 
public 

class 
ScriptsViewModel !
{		 
public

 
IEnumerable

 
<

 
ScriptViewModel

 *
>

* +
Scripts

, 3
{

4 5
get

6 9
;

9 :
set

; >
;

> ?
}

@ A
} 
} à
sD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Barebone\ViewModels\Shared\Scripts\ScriptsViewModelFactory.cs
	namespace		 	
Barebone		
 
.		 

ViewModels		 
.		 
Shared		 $
{

 
public 

class #
ScriptsViewModelFactory (
{ 
public 
ScriptsViewModel 
Create  &
(& '
)' (
{ 	
List 
< 
Script 
> 
scripts  
=! "
new# &
List' +
<+ ,
Script, 2
>2 3
(3 4
)4 5
;5 6
foreach 
( 
IExtensionMetadata '
extensionMetadata( 9
in: <
ExtensionManager= M
.M N
GetInstancesN Z
<Z [
IExtensionMetadata[ m
>m n
(n o
)o p
)p q
scripts 
. 
AddRange  
(  !
extensionMetadata! 2
.2 3
Scripts3 :
): ;
;; <
return 
new 
ScriptsViewModel '
(' (
)( )
{ 
Scripts 
= 
scripts !
.! "
OrderBy" )
() *
s* +
=>, .
s/ 0
.0 1
Position1 9
)9 :
.: ;
Select; A
(A B
s 
=> 
new "
ScriptViewModelFactory /
(/ 0
)0 1
.1 2
Create2 8
(8 9
s9 :
): ;
) 
} 
; 
} 	
} 
} Ù
jD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Barebone\ViewModels\Shared\Script\ScriptViewModel.cs
	namespace 	
Barebone
 
. 

ViewModels 
. 
Shared $
{ 
public 

class 
ScriptViewModel  
{ 
public 
string 
Url 
{ 
get 
;  
set! $
;$ %
}& '
}		 
}

 ı
qD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Barebone\ViewModels\Shared\Script\ScriptViewModelFactory.cs
	namespace 	
Barebone
 
. 

ViewModels 
. 
Shared $
{ 
public 

class "
ScriptViewModelFactory '
{		 
public

 
ScriptViewModel

 
Create

 %
(

% &
Script

& ,
script

- 3
)

3 4
{ 	
return 
new 
ScriptViewModel &
(& '
)' (
{ 
Url 
= 
script 
. 
Url  
} 
; 
} 	
} 
} œ
tD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Barebone\ViewModels\Shared\StyleSheets\StyleSheetsViewModel.cs
	namespace 	
Barebone
 
. 

ViewModels 
. 
Shared $
{ 
public 

class  
StyleSheetsViewModel %
{		 
public

 
IEnumerable

 
<

 
StyleSheetViewModel

 .
>

. /
StyleSheets

0 ;
{

< =
get

> A
;

A B
set

C F
;

F G
}

H I
} 
} ¿
{D:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Barebone\ViewModels\Shared\StyleSheets\StyleSheetsViewModelFactory.cs
	namespace		 	
Barebone		
 
.		 

ViewModels		 
.		 
Shared		 $
{

 
public 

class '
StyleSheetsViewModelFactory ,
{ 
public  
StyleSheetsViewModel #
Create$ *
(* +
)+ ,
{ 	
List 
< 

StyleSheet 
> 
styleSheets (
=) *
new+ .
List/ 3
<3 4

StyleSheet4 >
>> ?
(? @
)@ A
;A B
foreach 
( 
IExtensionMetadata '
extensionMetadata( 9
in: <
ExtensionManager= M
.M N
GetInstancesN Z
<Z [
IExtensionMetadata[ m
>m n
(n o
)o p
)p q
styleSheets 
. 
AddRange $
($ %
extensionMetadata% 6
.6 7
StyleSheets7 B
)B C
;C D
return 
new  
StyleSheetsViewModel +
(+ ,
), -
{ 
StyleSheets 
= 
styleSheets )
.) *
OrderBy* 1
(1 2
ss2 4
=>5 7
ss8 :
.: ;
Position; C
)C D
.D E
SelectE K
(K L
ss 
=> 
new &
StyleSheetViewModelFactory 4
(4 5
)5 6
.6 7
Create7 =
(= >
ss> @
)@ A
) 
} 
; 
} 	
} 
} Ä
rD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Barebone\ViewModels\Shared\StyleSheet\StyleSheetViewModel.cs
	namespace 	
Barebone
 
. 

ViewModels 
. 
Shared $
{ 
public 

class 
StyleSheetViewModel $
{ 
public 
string 
Url 
{ 
get 
;  
set! $
;$ %
}& '
}		 
}

 ï
yD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Barebone\ViewModels\Shared\StyleSheet\StyleSheetViewModelFactory.cs
	namespace 	
Barebone
 
. 

ViewModels 
. 
Shared $
{ 
public 

class &
StyleSheetViewModelFactory +
{		 
public

 
StyleSheetViewModel

 "
Create

# )
(

) *

StyleSheet

* 4

styleSheet

5 ?
)

? @
{ 	
return 
new 
StyleSheetViewModel *
(* +
)+ ,
{ 
Url 
= 

styleSheet  
.  !
Url! $
} 
; 
} 	
} 
} 