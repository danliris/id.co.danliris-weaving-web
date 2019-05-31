¸
}D:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Beams\CommandHandlers\AddBeamCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
Beams# (
.( )
CommandHandlers) 8
{ 
public 

class !
AddBeamCommandHandler &
: 	
ICommandHandler
 
< 
AddBeamCommand (
,( )
BeamDocument* 6
>6 7
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly 
IBeamRepository (
_beamRepository) 8
;8 9
public !
AddBeamCommandHandler $
($ %
IStorage% -
storage. 5
)5 6
{ 	
_storage 
= 
storage 
; 
_beamRepository 
= 
_storage &
.& '
GetRepository' 4
<4 5
IBeamRepository5 D
>D E
(E F
)F G
;G H
} 	
public 
async 
Task 
< 
BeamDocument &
>& '
Handle( .
(. /
AddBeamCommand/ =
request> E
,E F
CancellationToken/ @
cancellationTokenA R
)R S
{ 	
var 
existingBeamCode  
=! "
_beamRepository 
. 
Find 
( 
x 
=> 
x 
.  
Number  &
.& '
Equals' -
(- .
request. 5
.5 6
Number6 <
)< =
)= >
.   
FirstOrDefault   "
(  " #
)  # $
;  $ %
if"" 
("" 
existingBeamCode""  
!=""! #
null""$ (
)""( )
{## 
	Validator$$ 
.%% 
ErrorValidation%% $
(%%$ %
(%%% &
$str%%& .
,%%. /
$str&&& A
)&&A B
)&&B C
;&&C D
}'' 
var)) 
newBeam)) 
=)) 
new)) 
BeamDocument)) *
())* +
Guid))+ /
.))/ 0
NewGuid))0 7
())7 8
)))8 9
,))9 :
request**+ 2
.**2 3
Number**3 9
,**9 :
request+++ 2
.++2 3
Type++3 7
,++7 8
request,,+ 2
.,,2 3
EmptyWeight,,3 >
),,> ?
;,,? @
await.. 
_beamRepository.. !
...! "
Update.." (
(..( )
newBeam..) 0
)..0 1
;..1 2
_storage// 
.// 
Save// 
(// 
)// 
;// 
return11 
newBeam11 
;11 
}22 	
}33 
}44 Ù
ÄD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Beams\CommandHandlers\RemoveBeamCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
Beams# (
.( )
CommandHandlers) 8
{ 
public 

class $
RemoveBeamCommandHandler )
:* +
ICommandHandler, ;
<; <
RemoveBeamCommand< M
,M N
BeamDocumentO [
>[ \
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly 
IBeamRepository (
_beamRepository) 8
;8 9
public $
RemoveBeamCommandHandler '
(' (
IStorage( 0
storage1 8
)8 9
{ 	
_storage 
= 
storage 
; 
_beamRepository 
= 
_storage &
.& '
GetRepository' 4
<4 5
IBeamRepository5 D
>D E
(E F
)F G
;G H
} 	
public 
async 
Task 
< 
BeamDocument &
>& '
Handle( .
(. /
RemoveBeamCommand/ @
requestA H
,H I
CancellationTokenJ [
cancellationToken\ m
)m n
{ 	
var 
existingBeam 
= 
_beamRepository 
. 
Query 
. 
Where 
( 
x 
=> 
x  !
.! "
Identity" *
.* +
Equals+ 1
(1 2
request2 9
.9 :
Id: <
)< =
)= >
. 
Select 
( 
	readModel %
=>& (
new) ,
BeamDocument- 9
(9 :
	readModel: C
)C D
)D E
. 
FirstOrDefault #
(# $
)$ %
;% &
if!! 
(!! 
existingBeam!! 
==!! 
null!!  $
)!!$ %
{"" 
	Validator## 
.## 
ErrorValidation## )
(##) *
(##* +
$str##+ 7
,##7 8
$str##9 Z
+##[ \
existingBeam##] i
.##i j
Number##j p
)##r s
)##s t
;##t u
}$$ 
existingBeam&& 
.&& 
Remove&& 
(&&  
)&&  !
;&&! "
await(( 
_beamRepository(( !
.((! "
Update((" (
(((( )
existingBeam(() 5
)((5 6
;((6 7
_storage** 
.** 
Save** 
(** 
)** 
;** 
return,, 
existingBeam,, 
;,,  
}-- 	
}.. 
}// ∫ 
ÄD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Beams\CommandHandlers\UpdateBeamCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
Beams# (
.( )
CommandHandlers) 8
{ 
public 

class $
UpdateBeamCommandHandler )
: 	
ICommandHandler
 
< 
UpdateBeamCommand +
,+ ,
BeamDocument- 9
>9 :
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly 
IBeamRepository (
_beamRepository) 8
;8 9
public $
UpdateBeamCommandHandler '
(' (
IStorage( 0
storage1 8
)8 9
{ 	
_storage 
= 
storage 
; 
_beamRepository 
= 
_storage &
.& '
GetRepository' 4
<4 5
IBeamRepository5 D
>D E
(E F
)F G
;G H
} 	
public 
async 
Task 
< 
BeamDocument &
>& '
Handle( .
(. /
UpdateBeamCommand/ @
requestA H
,H I
CancellationToken/ @
cancellationTokenA R
)R S
{ 	
var 
existingBeam 
= 
_beamRepository 
. 
Find 
( 
x 
=> 
x 
.  
Identity  (
.( )
Equals) /
(/ 0
request0 7
.7 8
Id8 :
): ;
); <
. 
FirstOrDefault "
(" #
)# $
;$ %
if!! 
(!! 
existingBeam!! 
==!! 
null!!  $
)!!$ %
{"" 
	Validator## 
.$$ 
ErrorValidation$$ $
($$$ %
($$% &
$str$$& .
,$$. /
$str%%& G
+%%H I
existingBeam&&( 4
.&&4 5
Number&&5 ;
)&&; <
)&&< =
;&&= >
}'' 
var)) 
existingBeamCode))  
=))! "
_beamRepository** 
.++ 
Find++ 
(++ 
x++ 
=>++ 
x++ 
.++ 
Number++ %
.++% &
Equals++& ,
(++, -
request++- 4
.++4 5
Number++5 ;
)++; <
)++< =
.,, 
FirstOrDefault,, !
(,,! "
),," #
;,,# $
if.. 
(.. 
request.. 
... 
Number.. 
==.. !
existingBeam.." .
.... /
Number../ 5
&&..6 8
existingBeamCode..9 I
!=..J L
null..M Q
)..Q R
{// 
	Validator00 
.11 
ErrorValidation11 $
(11$ %
(11% &
$str11& .
,11. /
$str22& A
)22A B
)22B C
;22C D
}33 
existingBeam55 
.55 
SetBeamNumber55 &
(55& '
request55' .
.55. /
Number55/ 5
)555 6
;556 7
existingBeam66 
.66 
SetBeamType66 $
(66$ %
request66% ,
.66, -
Type66- 1
)661 2
;662 3
existingBeam77 
.77 
SetEmptyWeight77 '
(77' (
request77( /
.77/ 0
EmptyWeight770 ;
)77; <
;77< =
await99 
_beamRepository99 !
.99! "
Update99" (
(99( )
existingBeam99) 5
)995 6
;996 7
_storage:: 
.:: 
Save:: 
(:: 
):: 
;:: 
return<< 
existingBeam<< 
;<<  
}== 	
}>> 
}?? î$
öD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\DailyOperations\Loom\CommandHandlers\AddDailyOperationLoomCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
DailyOperations# 2
.2 3
Loom3 7
.7 8
CommandHandlers8 G
{ 
public 

class /
#AddDailyOperationLoomCommandHandler 4
: 	
ICommandHandler
 
< +
AddNewDailyOperationLoomCommand 9
,9 :&
DailyOperationLoomDocument 4
>4 5
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly )
IDailyOperationLoomRepository 6/
#_dailyOperationalDocumentRepository /
;/ 0
private 
readonly +
IDailyOperationSizingRepository 8+
_dailyOperationSizingRepository +
;+ ,
public /
#AddDailyOperationLoomCommandHandler 2
(2 3
IStorage3 ;
storage< C
)C D
{ 	
_storage 
= 
storage 
; /
#_dailyOperationalDocumentRepository /
=0 1
_storage 
. 
GetRepository &
<& ')
IDailyOperationLoomRepository' D
>D E
(E F
)F G
;G H+
_dailyOperationSizingRepository +
=, -
_storage   
.   
GetRepository   &
<  & '+
IDailyOperationSizingRepository  ' F
>  F G
(  G H
)  H I
;  I J
}!! 	
public## 
async## 
Task## 
<## &
DailyOperationLoomDocument## 4
>##4 5
Handle$$ 
($$ +
AddNewDailyOperationLoomCommand$$ 2
request$$3 :
,$$: ;
CancellationToken%% $
cancellationToken%%% 6
)%%6 7
{&& 	
var'' )
dailyOperationMachineDocument'' -
=''. /
new(( &
DailyOperationLoomDocument(( .
(((. /
Guid((/ 3
.((3 4
NewGuid((4 ;
(((; <
)((< =
,((= >
request))/ 6
.))6 7
UnitId))7 =
,))= >
request**/ 6
.**6 7
	MachineId**7 @
,**@ A
request++/ 6
.++6 7
BeamId++7 =
,++= >
request,,/ 6
.,,6 7
OrderId,,7 >
,,,> ?
request--/ 6
.--6 7&
DailyOperationMonitoringId--7 Q
,--Q R'
DailyOperationMachineStatus../ J
...J K
ONENTRY..K R
)..R S
;..S T
var00 $
newDailyOperationHistory00 (
=00) *
new11 %
DailyOperationLoomHistory11 -
(11- .
request11. 5
.115 6
PreparationDate116 E
.11E F
Date11F J
,11J K
TimeSpan22. 6
.226 7
Parse227 <
(22< =
request22= D
.22D E
PreparationTime22E T
)22T U
,22U V'
DailyOperationMachineStatus33. I
.33I J
ONENTRY33J Q
,33Q R
true44. 2
,442 3
false55. 3
)553 4
;554 5
var77 
newOperation77 
=77 
new88 $
DailyOperationLoomDetail88 /
(88/ 0
Guid880 4
.884 5
NewGuid885 <
(88< =
)88= >
,88> ?
request990 7
.997 8
ShiftId998 ?
,99? @
request::0 7
.::7 8

OperatorId::8 B
,::B C
request;;0 7
.;;7 8

WarpOrigin;;8 B
,;;B C
request<<0 7
.<<7 8

WeftOrigin<<8 B
,<<B C$
newDailyOperationHistory==0 H
)==H I
;==I J)
dailyOperationMachineDocument?? )
.??) **
AddDailyOperationMachineDetail??* H
(??H I
newOperation??I U
)??U V
;??V W
awaitAA /
#_dailyOperationalDocumentRepositoryAA 5
.AA5 6
UpdateAA6 <
(AA< =)
dailyOperationMachineDocumentAA= Z
)AAZ [
;AA[ \
_storageCC 
.CC 
SaveCC 
(CC 
)CC 
;CC 
returnEE )
dailyOperationMachineDocumentEE 0
;EE0 1
}FF 	
}GG 
}HH ‘'
ûD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\DailyOperations\Sizing\CommandHandlers\NewEntryOperationSizingCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
DailyOperations# 2
.2 3
Sizing3 9
.9 :
CommandHandlers: I
{ 
public 

class 1
%NewEntryOperationSizingCommandHandler 6
:7 8
ICommandHandler9 H
<H I/
#NewEntryDailyOperationSizingCommandI l
,l m)
DailyOperationSizingDocument	n ä
>
ä ã
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly +
IDailyOperationSizingRepository 83
'_dailyOperationSizingDocumentRepository 3
;3 4
public 1
%NewEntryOperationSizingCommandHandler 4
(4 5
IStorage5 =
storage> E
)E F
{ 	
_storage 
= 
storage 
; 3
'_dailyOperationSizingDocumentRepository 3
=4 5
_storage 
. 
GetRepository &
<& '+
IDailyOperationSizingRepository' F
>F G
(G H
)H I
;I J
} 	
public 
async 
Task 
< (
DailyOperationSizingDocument 6
>6 7
Handle 
( /
#NewEntryDailyOperationSizingCommand 6
request7 >
,> ?
CancellationToken   $
cancellationToken  % 6
)  6 7
{!! 	
var'' (
dailyOperationSizingDocument'' ,
=''- .
new(( (
DailyOperationSizingDocument(( 0
(((0 1
Guid((1 5
.((5 6
NewGuid((6 =
(((= >
)((> ?
,((? @
request))4 ;
.)); <
MachineDocumentId))< M
,))M N
request**4 ;
.**; <
WeavingUnitId**< I
,**I J
request++4 ;
.++; <"
ConstructionDocumentId++< R
,++R S
request,,4 ;
.,,; <

RecipeCode,,< F
,,,F G
new--4 72
&DailyOperationSizingCounterValueObject--8 ^
(--^ _
request--_ f
.--f g
Counter--g n
.--n o
Start--o t
,--t u
$str--v x
)--x y
,--y z
new..4 71
%DailyOperationSizingWeightValueObject..8 ]
(..] ^
request..^ e
...e f
Weight..f l
...l m
Netto..m r
,..r s
$str..t v
)..v w
,..w x
new//4 7
List//8 <
<//< =
BeamId//= C
>//C D
(//D E
request//E L
.//L M+
WarpingBeamCollectionDocumentId//M l
)//l m
,//m n
$num004 5
,005 6
$num114 5
,115 6
$num224 5
,225 6
$num334 5
,335 6
$num444 5
,445 6
new554 7
BeamId558 >
(55> ?
Guid55? C
.55C D
Empty55D I
)55I J
)55J K
;55K L
var77 
newOperation77  
=77! "
new88 &
DailyOperationSizingDetail88 2
(882 3
Guid883 7
.887 8
NewGuid888 ?
(88? @
)88@ A
,88A B
request993 :
.99: ;
Details99; B
.99B C
ShiftId99C J
,99J K
request::3 :
.::: ;
Details::; B
.::B C
OperatorDocumentId::C U
,::U V
new;;3 62
&DailyOperationSizingHistoryValueObject;;7 ]
(;;] ^
request;;^ e
.;;e f
Details;;f m
.;;m n
History;;n u
.;;u v
TimeOnMachine	;;v É
,
;;É Ñ)
DailyOperationMachineStatus
;;Ö †
.
;;† °
ONENTRY
;;° ®
,
;;® ©
request
;;™ ±
.
;;± ≤
Details
;;≤ π
.
;;π ∫
History
;;∫ ¡
.
;;¡ ¬
Information
;;¬ Õ
)
;;Õ Œ
,
;;Œ œ
new<<3 61
%DailyOperationSizingCausesValueObject<<7 \
(<<\ ]
$str<<] _
,<<_ `
$str<<` b
)<<b c
)<<c d
;<<d e(
dailyOperationSizingDocument>> ,
.>>, -)
AddDailyOperationSizingDetail>>- J
(>>J K
newOperation>>K W
)>>W X
;>>X Y
await@@ 3
'_dailyOperationSizingDocumentRepository@@ 9
.@@9 :
Update@@: @
(@@@ A(
dailyOperationSizingDocument@@A ]
)@@] ^
;@@^ _
_storageBB 
.BB 
SaveBB 
(BB 
)BB 
;BB 
returnDD (
dailyOperationSizingDocumentDD /
;DD/ 0
}EE 	
}FF 
}GG æ8
•D:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\DailyOperations\Sizing\CommandHandlers\UpdateDoffDailyOperationSizingCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
DailyOperations# 2
.2 3
Sizing3 9
.9 :
CommandHandlers: I
{ 
public 

class 8
,UpdateDoffDailyOperationSizingCommandHandler =
:> ?
ICommandHandler@ O
<O P7
+UpdateDoffFinishDailyOperationSizingCommandP {
,{ |)
DailyOperationSizingDocument	} ô
>
ô ö
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly +
IDailyOperationSizingRepository 83
'_dailyOperationSizingDocumentRepository 3
;3 4
public 8
,UpdateDoffDailyOperationSizingCommandHandler ;
(; <
IStorage< D
storageE L
)L M
{ 	
_storage 
= 
storage 
; 3
'_dailyOperationSizingDocumentRepository 3
=4 5
_storage6 >
.> ?
GetRepository? L
<L M+
IDailyOperationSizingRepositoryM l
>l m
(m n
)n o
;o p
} 	
public 
async 
Task 
< (
DailyOperationSizingDocument 6
>6 7
Handle8 >
(> ?7
+UpdateDoffFinishDailyOperationSizingCommand? j
requestk r
,r s
CancellationToken	t Ö
cancellationToken
Ü ó
)
ó ò
{   	
var!! 
query!! 
=!! 3
'_dailyOperationSizingDocumentRepository!! ?
.!!? @
Query!!@ E
.!!E F
Include!!F M
(!!M N
d!!N O
=>!!P R
d!!S T
.!!T U'
DailyOperationSizingDetails!!U p
)!!p q
.!!q r
Where!!r w
(!!w x
entity!!x ~
=>	!! Å
entity
!!Ç à
.
!!à â
Identity
!!â ë
.
!!ë í
Equals
!!í ò
(
!!ò ô
request
!!ô †
.
!!† °
Id
!!° £
)
!!£ §
)
!!§ •
;
!!• ¶
var"" "
existingDailyOperation"" &
=""' (3
'_dailyOperationSizingDocumentRepository"") P
.""P Q
Find""Q U
(""U V
query""V [
)""[ \
.""\ ]
FirstOrDefault""] k
(""k l
)""l m
;""m n
var## 
lastHistory## 
=## "
existingDailyOperation## 4
.##4 5'
DailyOperationSizingDetails##5 P
.##P Q
Last##Q U
(##U V
)##V W
;##W X
var%% (
dailyOperationSizingDocument%% ,
=%%- .
new&& (
DailyOperationSizingDocument&& 0
(&&0 1
Guid&&1 5
.&&5 6
NewGuid&&6 =
(&&= >
)&&> ?
,&&? @"
existingDailyOperation''1 G
.''G H
MachineDocumentId''H Y
,''Y Z"
existingDailyOperation((1 G
.((G H
WeavingUnitId((H U
,((U V"
existingDailyOperation))1 G
.))G H"
ConstructionDocumentId))H ^
,))^ _"
existingDailyOperation**1 G
.**G H

RecipeCode**H R
,**R S
new++1 42
&DailyOperationSizingCounterValueObject++5 [
(++[ \"
existingDailyOperation++\ r
.++r s
Counter++s z
.++z {
Deserialize	++{ Ü
<
++Ü á0
"DailyOperationSizingCounterCommand
++á ©
>
++© ™
(
++™ ´
)
++´ ¨
)
++¨ ≠
,
++≠ Æ
new,,1 41
%DailyOperationSizingWeightValueObject,,5 Z
(,,Z ["
existingDailyOperation,,[ q
.,,q r
Weight,,r x
.,,x y
Deserialize	,,y Ñ
<
,,Ñ Ö/
!DailyOperationSizingWeightCommand
,,Ö ¶
>
,,¶ ß
(
,,ß ®
)
,,® ©
)
,,© ™
,
,,™ ´
new--1 4
List--5 9
<--9 :
BeamId--: @
>--@ A
(--A B"
existingDailyOperation--B X
.--X Y+
WarpingBeamCollectionDocumentId--Y x
.--x y
Deserialize	--y Ñ
<
--Ñ Ö
List
--Ö â
<
--â ä
BeamId
--ä ê
>
--ê ë
>
--ë í
(
--í ì
)
--ì î
)
--î ï
,
--ï ñ
request..1 8
...8 9
MachineSpeed..9 E
,..E F
request//1 8
.//8 9
TexSQ//9 >
,//> ?
request001 8
.008 9
Visco009 >
,00> ?
request111 8
.118 9
PIS119 <
,11< =
request221 8
.228 9
SPU229 <
,22< =
new331 4
BeamId335 ;
(33; <
request33< C
.33C D 
SizingBeamDocumentId33D X
.33X Y
Value33Y ^
)33^ _
)33_ `
;33` a
var55 
newOperation55 
=55 
new66 &
DailyOperationSizingDetail66 6
(666 7
Guid667 ;
.66; <
NewGuid66< C
(66C D
)66D E
,66E F
new777 :
ShiftId77; B
(77B C
lastHistory77C N
.77N O
ShiftDocumentId77O ^
)77^ _
,77_ `
new887 :

OperatorId88; E
(88E F
lastHistory88F Q
.88Q R
OperatorDocumentId88R d
)88d e
,88e f
new997 :2
&DailyOperationSizingHistoryValueObject99; a
(99a b
request99b i
.99i j
Details99j q
.99q r
History99r y
.99y z
TimeOnMachine	99z á
,
99á à)
DailyOperationMachineStatus
99â §
.
99§ •
ONFINISH
99• ≠
,
99≠ Æ
request
99Ø ∂
.
99∂ ∑
Details
99∑ æ
.
99æ ø
History
99ø ∆
.
99∆ «
Information
99« “
)
99“ ”
,
99” ‘
new::7 :1
%DailyOperationSizingCausesValueObject::; `
(::` a
lastHistory::a l
.::l m
Causes::m s
.::s t
Deserialize::t 
<	:: Ä/
!DailyOperationSizingCausesCommand
::Ä °
>
::° ¢
(
::¢ £
)
::£ §
)
::§ •
)
::• ¶
;
::¶ ß(
dailyOperationSizingDocument<< (
.<<( ))
AddDailyOperationSizingDetail<<) F
(<<F G
newOperation<<G S
)<<S T
;<<T U
await>> 3
'_dailyOperationSizingDocumentRepository>> =
.>>= >
Update>>> D
(>>D E(
dailyOperationSizingDocument>>E a
)>>a b
;>>b c
_storage?? 
.?? 
Save?? 
(?? 
)?? 
;??  
returnAA "
existingDailyOperationAA )
;AA) *
}BB 	
}CC 
}DD ‚%
¶D:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\DailyOperations\Sizing\CommandHandlers\UpdatePauseDailyOperationSizingCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
DailyOperations# 2
.2 3
Sizing3 9
.9 :
CommandHandlers: I
{ 
public 

class 9
-UpdatePauseDailyOperationSizingCommandHandler >
:? @
ICommandHandlerA P
<P Q2
&UpdatePauseDailyOperationSizingCommandQ w
,w x)
DailyOperationSizingDocument	y ï
>
ï ñ
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly +
IDailyOperationSizingRepository 83
'_dailyOperationSizingDocumentRepository 3
;3 4
public 9
-UpdatePauseDailyOperationSizingCommandHandler <
(< =
IStorage= E
storageF M
)M N
{ 	
_storage 
= 
storage 
; 3
'_dailyOperationSizingDocumentRepository 3
=4 5
_storage6 >
.> ?
GetRepository? L
<L M+
IDailyOperationSizingRepositoryM l
>l m
(m n
)n o
;o p
} 	
public 
async 
Task 
< (
DailyOperationSizingDocument 6
>6 7
Handle8 >
(> ?2
&UpdatePauseDailyOperationSizingCommand? e
requestf m
,m n
CancellationToken	o Ä
cancellationToken
Å í
)
í ì
{ 	
var   
query   
=   3
'_dailyOperationSizingDocumentRepository   ?
.  ? @
Query  @ E
.  E F
Include  F M
(  M N
d  N O
=>  P R
d  S T
.  T U'
DailyOperationSizingDetails  U p
)  p q
.  q r
Where  r w
(  w x
entity  x ~
=>	   Å
entity
  Ç à
.
  à â
Identity
  â ë
.
  ë í
Equals
  í ò
(
  ò ô
request
  ô †
.
  † °
Id
  ° £
)
  £ §
)
  § •
;
  • ¶
var!! "
existingDailyOperation!! &
=!!' (3
'_dailyOperationSizingDocumentRepository!!) P
.!!P Q
Find!!Q U
(!!U V
query!!V [
)!![ \
.!!\ ]
FirstOrDefault!!] k
(!!k l
)!!l m
;!!m n
var"" 
lastHistory"" 
="" "
existingDailyOperation"" 4
.""4 5'
DailyOperationSizingDetails""5 P
.""P Q
Last""Q U
(""U V
)""V W
;""W X
var$$ 
newOperation$$  
=$$! "
new%% &
DailyOperationSizingDetail%% 6
(%%6 7
Guid%%7 ;
.%%; <
NewGuid%%< C
(%%C D
)%%D E
,%%E F
new&&7 :
ShiftId&&; B
(&&B C
lastHistory&&C N
.&&N O
ShiftDocumentId&&O ^
)&&^ _
,&&_ `
new''7 :

OperatorId''; E
(''E F
lastHistory''F Q
.''Q R
OperatorDocumentId''R d
)''d e
,''e f
new((7 :2
&DailyOperationSizingHistoryValueObject((; a
(((a b
request((b i
.((i j
Details((j q
.((q r
History((r y
.((y z
TimeOnMachine	((z á
,
((á à)
DailyOperationMachineStatus
((â §
.
((§ •
ONSTOP
((• ´
,
((´ ¨
request
((≠ ¥
.
((¥ µ
Details
((µ º
.
((º Ω
History
((Ω ƒ
.
((ƒ ≈
Information
((≈ –
)
((– —
,
((— “
new))7 :1
%DailyOperationSizingCausesValueObject)); `
())` a
request))a h
.))h i
Details))i p
.))p q
Causes))q w
.))w x

BrokenBeam	))x Ç
,
))Ç É
request
))É ä
.
))ä ã
Details
))ã í
.
))í ì
Causes
))ì ô
.
))ô ö
MachineTroubled
))ö ©
)
))© ™
)
))™ ´
;
))´ ¨
await++ 3
'_dailyOperationSizingDocumentRepository++ =
.++= >
Update++> D
(++D E"
existingDailyOperation++E [
)++[ \
;++\ ]
_storage,, 
.,, 
Save,, 
(,, 
),, 
;,,  
return// "
existingDailyOperation// )
;//) *
}00 	
}11 
}22 ÿ%
ßD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\DailyOperations\Sizing\CommandHandlers\UpdateResumeDailyOperationSizingCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
DailyOperations# 2
.2 3
Sizing3 9
.9 :
CommandHandlers: I
{ 
public 

class :
.UpdateResumeDailyOperationSizingCommandHandler ?
:@ A
ICommandHandlerB Q
<Q R3
'UpdateResumeDailyOperationSizingCommandR y
,y z)
DailyOperationSizingDocument	{ ó
>
ó ò
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly +
IDailyOperationSizingRepository 83
'_dailyOperationSizingDocumentRepository 3
;3 4
public :
.UpdateResumeDailyOperationSizingCommandHandler =
(= >
IStorage> F
storageG N
)N O
{ 	
_storage 
= 
storage 
; 3
'_dailyOperationSizingDocumentRepository 3
=4 5
_storage6 >
.> ?
GetRepository? L
<L M+
IDailyOperationSizingRepositoryM l
>l m
(m n
)n o
;o p
} 	
public 
async 
Task 
< (
DailyOperationSizingDocument 6
>6 7
Handle8 >
(> ?3
'UpdateResumeDailyOperationSizingCommand? f
requestg n
,n o
CancellationToken	p Å
cancellationToken
Ç ì
)
ì î
{ 	
var   
query   
=   3
'_dailyOperationSizingDocumentRepository   ?
.  ? @
Query  @ E
.  E F
Include  F M
(  M N
d  N O
=>  P R
d  S T
.  T U'
DailyOperationSizingDetails  U p
)  p q
.  q r
Where  r w
(  w x
entity  x ~
=>	   Å
entity
  Ç à
.
  à â
Identity
  â ë
.
  ë í
Equals
  í ò
(
  ò ô
request
  ô †
.
  † °
Id
  ° £
)
  £ §
)
  § •
;
  • ¶
var!! "
existingDailyOperation!! &
=!!' (3
'_dailyOperationSizingDocumentRepository!!) P
.!!P Q
Find!!Q U
(!!U V
query!!V [
)!![ \
.!!\ ]
FirstOrDefault!!] k
(!!k l
)!!l m
;!!m n
var"" 
lastHistory"" 
="" "
existingDailyOperation"" 4
.""4 5'
DailyOperationSizingDetails""5 P
.""P Q
Last""Q U
(""U V
)""V W
;""W X
var$$ 
newOperation$$ 
=$$ 
new%% &
DailyOperationSizingDetail%% 6
(%%6 7
Guid%%7 ;
.%%; <
NewGuid%%< C
(%%C D
)%%D E
,%%E F
new&&7 :
ShiftId&&; B
(&&B C
lastHistory&&C N
.&&N O
ShiftDocumentId&&O ^
)&&^ _
,&&_ `
new''7 :

OperatorId''; E
(''F G
request''G N
.''N O
Details''O V
.''V W
OperatorDocumentId''W i
.''i j
Value''j o
)''o p
,''p q
new((7 :2
&DailyOperationSizingHistoryValueObject((; a
(((a b
request((b i
.((i j
Details((j q
.((q r
History((r y
.((y z
TimeOnMachine	((z á
,
((á à)
DailyOperationMachineStatus
((â §
.
((§ •
ONRESUME
((• ≠
,
((≠ Æ
request
((Ø ∂
.
((∂ ∑
Details
((∑ æ
.
((æ ø
History
((ø ∆
.
((∆ «
Information
((« “
)
((“ ”
,
((” ‘
new))7 :1
%DailyOperationSizingCausesValueObject)); `
())` a
lastHistory))a l
.))l m
Causes))m s
.))s t
Deserialize))t 
<	)) Ä/
!DailyOperationSizingCausesCommand
))Ä °
>
))° ¢
(
))¢ £
)
))£ §
)
))§ •
)
))• ¶
;
))¶ ß
await++ 3
'_dailyOperationSizingDocumentRepository++ =
.++= >
Update++> D
(++D E"
existingDailyOperation++E [
)++[ \
;++\ ]
_storage,, 
.,, 
Save,, 
(,, 
),, 
;,,  
return// "
existingDailyOperation// )
;//) *
}00 	
}11 
}22 ¶(
¶D:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\DailyOperations\Sizing\CommandHandlers\UpdateStartDailyOperationSizingCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
DailyOperations# 2
.2 3
Sizing3 9
.9 :
CommandHandlers: I
{ 
public 

class 9
-UpdateStartDailyOperationSizingCommandHandler >
:? @
ICommandHandlerA P
<P Q2
&UpdateStartDailyOperationSizingCommandQ w
,w x)
DailyOperationSizingDocument	y ï
>
ï ñ
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly +
IDailyOperationSizingRepository 83
'_dailyOperationSizingDocumentRepository 3
;3 4
public 9
-UpdateStartDailyOperationSizingCommandHandler <
(< =
IStorage= E
storageF M
)M N
{ 	
_storage 
= 
storage 
; 3
'_dailyOperationSizingDocumentRepository 3
=4 5
_storage6 >
.> ?
GetRepository? L
<L M+
IDailyOperationSizingRepositoryM l
>l m
(m n
)n o
;o p
} 	
public   
async   
Task   
<   (
DailyOperationSizingDocument   6
>  6 7
Handle  8 >
(  > ?2
&UpdateStartDailyOperationSizingCommand  ? e
request  f m
,  m n
CancellationToken	  o Ä
cancellationToken
  Å í
)
  í ì
{!! 	
var"" 
query"" 
="" 3
'_dailyOperationSizingDocumentRepository"" ?
.""? @
Query""@ E
.""E F
Include""F M
(""M N
d""N O
=>""P R
d""S T
.""T U'
DailyOperationSizingDetails""U p
)""p q
.""q r
Where""r w
(""w x
entity""x ~
=>	"" Å
entity
""Ç à
.
""à â
Identity
""â ë
.
""ë í
Equals
""í ò
(
""ò ô
request
""ô †
.
""† °
Id
""° £
)
""£ §
)
""§ •
;
""• ¶
var## "
existingDailyOperation## &
=##' (3
'_dailyOperationSizingDocumentRepository##) P
.##P Q
Find##Q U
(##U V
query##V [
)##[ \
.##\ ]
FirstOrDefault##] k
(##k l
)##l m
;##m n
var$$ 
lastHistory$$ 
=$$ "
existingDailyOperation$$ 4
.$$4 5'
DailyOperationSizingDetails$$5 P
.$$P Q
Last$$Q U
($$U V
)$$V W
;$$W X
var&& 
newOperation&& 
=&& 
new'' &
DailyOperationSizingDetail'' 2
(''2 3
Guid''3 7
.''7 8
NewGuid''8 ?
(''? @
)''@ A
,''A B
new((3 6
ShiftId((7 >
(((> ?
request((? F
.((F G
Details((G N
.((N O
ShiftDocumentId((O ^
.((^ _
Value((_ d
)((d e
,((e f
new))3 6

OperatorId))7 A
())A B
lastHistory))B M
.))M N
OperatorDocumentId))N `
)))` a
,))a b
new**3 62
&DailyOperationSizingHistoryValueObject**7 ]
(**] ^
request**^ e
.**e f
Details**f m
.**m n
History**n u
.**u v
TimeOnMachine	**v É
,
**É Ñ)
DailyOperationMachineStatus
**Ö †
.
**† °
ONSTOP
**° ß
,
**ß ®
request
**© ∞
.
**∞ ±
Details
**± ∏
.
**∏ π
History
**π ¿
.
**¿ ¡
Information
**¡ Ã
)
**Ã Õ
,
**Õ Œ
new++3 61
%DailyOperationSizingCausesValueObject++7 \
(++\ ]
lastHistory++] h
.++h i
Causes++i o
.++o p
Deserialize++p {
<++{ |2
%DailyOperationSizingCausesValueObject	++| °
>
++° ¢
(
++¢ £
)
++£ §
.
++§ •

BrokenBeam
++• Ø
,
++Ø ∞
lastHistory
++± º
.
++º Ω
Causes
++Ω √
.
++√ ƒ
Deserialize
++ƒ œ
<
++œ –3
%DailyOperationSizingCausesValueObject
++– ı
>
++ı ˆ
(
++ˆ ˜
)
++˜ ¯
.
++¯ ˘
MachineTroubled
++˘ à
)
++à â
)
++â ä
;
++ä ã
await-- 3
'_dailyOperationSizingDocumentRepository-- 9
.--9 :
Update--: @
(--@ A"
existingDailyOperation--A W
)--W X
;--X Y
_storage.. 
... 
Save.. 
(.. 
).. 
;.. 
return11 "
existingDailyOperation11 )
;11) *
}22 	
}33 
}44 ∆7
ïD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Estimations\Productions\CommandHandlers\AddEstimationCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
Estimations# .
.. /
Productions/ :
.: ;
CommandHandlers; J
{ 
public 

class '
AddEstimationCommandHandler ,
:- .
ICommandHandler/ >
<> ?#
AddNewEstimationCommand? V
,V W'
EstimatedProductionDocumentX s
>s t
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly (
IEstimationProductRepository 5(
_estimationProductRepository6 R
;R S
private 
readonly )
IFabricConstructionRepository 6+
_constructionDocumentRepository7 V
;V W
private 
readonly +
IWeavingOrderDocumentRepository 8+
_weavingOrderDocumentRepository9 X
;X Y
public '
AddEstimationCommandHandler *
(* +
IStorage+ 3
storage4 ;
); <
{ 	
_storage 
= 
storage 
; (
_estimationProductRepository (
=) *
_storage+ 3
.3 4
GetRepository4 A
<A B(
IEstimationProductRepositoryB ^
>^ _
(_ `
)` a
;a b+
_constructionDocumentRepository +
=, -
_storage. 6
.6 7
GetRepository7 D
<D E)
IFabricConstructionRepositoryE b
>b c
(c d
)d e
;e f+
_weavingOrderDocumentRepository +
=, -
_storage. 6
.6 7
GetRepository7 D
<D E+
IWeavingOrderDocumentRepositoryE d
>d e
(e f
)f g
;g h
}   	
public"" 
async"" 
Task"" 
<"" '
EstimatedProductionDocument"" 5
>""5 6
Handle""7 =
(""= >#
AddNewEstimationCommand""> U
request""V ]
,""] ^
CancellationToken""_ p
cancellationToken	""q Ç
)
""Ç É
{## 	
var$$ 
estimationNumber$$  
=$$! "
await$$# ((
_estimationProductRepository$$) E
.$$E F
GetEstimationNumber$$F Y
($$Y Z
)$$Z [
;$$[ \
var&& '
estimatedProductionDocument&& +
=&&, -
new&&. 1'
EstimatedProductionDocument&&2 M
(&&M N
Guid&&N R
.&&R S
NewGuid&&S Z
(&&Z [
)&&[ \
,&&\ ]
estimationNumber&&^ n
,&&n o
request&&p w
.&&w x
Period&&x ~
,&&~ 
new
&&Ä É
UnitId
&&Ñ ä
(
&&ä ã
request
&&ã í
.
&&í ì
Unit
&&ì ó
.
&&ó ò
Id
&&ò ö
)
&&ö õ
)
&&õ ú
;
&&ú ù
foreach(( 
((( 
var(( 
product(( 
in((  "
request((# *
.((* +
EstimationProducts((+ =
)((= >
{)) 
var** )
exsistingConstructionDocument** 1
=**2 3+
_constructionDocumentRepository**4 S
.**S T
Find**T X
(**X Y
o**Y Z
=>**[ ]
o**^ _
.**_ `
ConstructionNumber**` r
.**r s
Equals**s y
(**y z
product	**z Å
.
**Å Ç 
ConstructionNumber
**Ç î
)
**î ï
)
**ï ñ
.
**ñ ó
FirstOrDefault
**ó •
(
**• ¶
)
**¶ ß
;
**ß ®
var++ 
existingOrder++ !
=++" #+
_weavingOrderDocumentRepository++$ C
.++C D
Find++D H
(++H I
o++I J
=>++K M
o++N O
.++O P
OrderNumber++P [
.++[ \
Equals++\ b
(++b c
product++c j
.++j k
OrderNumber++k v
)++v w
)++w x
.++x y
FirstOrDefault	++y á
(
++á à
)
++à â
;
++â ä
var--  
constructionDocument-- (
=--) *
new--+ . 
ConstructionDocument--/ C
(--C D)
exsistingConstructionDocument--D a
.--a b
Identity--b j
,--j k)
exsistingConstructionDocument..< Y
...Y Z
ConstructionNumber..Z l
,..l m)
exsistingConstructionDocument//< Y
.//Y Z
	TotalYarn//Z c
)//c d
;//d e
var00 
productGrade00  
=00! "
new00# &
ProductGrade00' 3
(003 4
product004 ;
.00; <
GradeA00< B
,00B C
product00D K
.00K L
GradeB00L R
,00R S
product00T [
.00[ \
GradeC00\ b
,00b c
product00d k
.00k l
GradeD00l r
)00r s
;00s t
var11 
order11 
=11 
new11 $
OrderDocumentValueObject11  8
(118 9
existingOrder119 F
.11F G
Identity11G O
,11O P
existingOrder229 F
.22F G
OrderNumber22G R
,22R S
existingOrder339 F
.33F G

WholeGrade33G Q
,33Q R 
constructionDocument449 M
,44M N
existingOrder559 F
.55F G
DateOrdered55G R
)55R S
;55S T
var66 

newProduct66 
=66  
new66! $
EstimationProduct66% 6
(666 7
Guid667 ;
.66; <
NewGuid66< C
(66C D
)66D E
,66E F
order66G L
,66L M
productGrade66N Z
,66Z [
product66\ c
.66c d
TotalGramEstimation66d w
)66w x
;66x y'
estimatedProductionDocument88 +
.88+ , 
AddEstimationProduct88, @
(88@ A

newProduct88A K
)88K L
;88L M
existingOrder99 
.99 
SetOrderStatus99 ,
(99, -
	Constants99- 6
.996 7
ONESTIMATED997 B
)99B C
;99C D
await:: +
_weavingOrderDocumentRepository:: 5
.::5 6
Update::6 <
(::< =
existingOrder::= J
)::J K
;::K L
};; 
await== (
_estimationProductRepository== .
.==. /
Update==/ 5
(==5 6'
estimatedProductionDocument==6 Q
)==Q R
;==R S
_storage?? 
.?? 
Save?? 
(?? 
)?? 
;?? 
returnAA '
estimatedProductionDocumentAA .
;AA. /
}BB 	
}CC 
}DD Ê'
òD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Estimations\Productions\CommandHandlers\RemoveEstimationCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
Estimations# .
.. /
Productions/ :
.: ;
CommandHandlers; J
{ 
public 

class *
RemoveEstimationCommandHandler /
:0 1
ICommandHandler2 A
<A B*
RemoveEstimationProductCommandB `
,` a'
EstimatedProductionDocumentb }
>} ~
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly (
IEstimationProductRepository 5(
_estimationProductRepository6 R
;R S
private 
readonly +
IWeavingOrderDocumentRepository 8+
_weavingOrderDocumentRepository9 X
;X Y
public *
RemoveEstimationCommandHandler -
(- .
IStorage. 6
storage7 >
)> ?
{ 	
_storage 
= 
storage 
; (
_estimationProductRepository (
=) *
_storage+ 3
.3 4
GetRepository4 A
<A B(
IEstimationProductRepositoryB ^
>^ _
(_ `
)` a
;a b+
_weavingOrderDocumentRepository +
=, -
_storage. 6
.6 7
GetRepository7 D
<D E+
IWeavingOrderDocumentRepositoryE d
>d e
(e f
)f g
;g h
} 	
public 
async 
Task 
< '
EstimatedProductionDocument 5
>5 6
Handle7 =
(= >*
RemoveEstimationProductCommand> \
request] d
,d e
CancellationTokenf w
cancellationToken	x â
)
â ä
{   	
var!! 
query!! 
=!! (
_estimationProductRepository!! 4
.!!4 5
Query!!5 :
.!!: ;
Include!!; B
(!!B C
o!!C D
=>!!E G
o!!H I
.!!I J
EstimationProducts!!J \
)!!\ ]
;!!] ^
var"" 
exsistingEstimation"" #
=""$ %(
_estimationProductRepository""& B
.""B C
Find""C G
(""G H
query""H M
)""M N
.""N O
Where""O T
(""T U
entity""U [
=>""\ ^
entity""_ e
.""e f
Identity""f n
.""n o
Equals""o u
(""u v
request""v }
.""} ~
Id	""~ Ä
)
""Ä Å
)
""Å Ç
.
""Ç É
FirstOrDefault
""É ë
(
""ë í
)
""í ì
;
""ì î
if$$ 
($$ 
exsistingEstimation$$ #
==$$$ &
null$$' +
)$$+ ,
{%% 
	Validator&& 
.&& 
ErrorValidation&& )
(&&) *
(&&* +
$str&&+ @
,&&@ A
$str&&B v
+&&w x
request	&&y Ä
.
&&Ä Å
Id
&&Å É
)
&&É Ñ
)
&&Ñ Ö
;
&&Ö Ü
}'' 
foreach)) 
()) 
var)) 
estimatedProduct)) (
in))) +
exsistingEstimation)), ?
.))? @
EstimationProducts))@ R
)))R S
{** 
var++ 
order++ 
=++ +
_weavingOrderDocumentRepository++ ;
.++; <
Find++< @
(++@ A
e++A B
=>++C E
e++F G
.++G H
OrderNumber++H S
.++S T
Equals++T Z
(++Z [
estimatedProduct++[ k
.++k l
OrderDocument++l y
.,,k l
Deserialize,,l w
<,,w x)
EstimationProductValueObject	,,x î
>
,,î ï
(
,,ï ñ
)
,,ñ ó
.--k l
OrderNumber--l w
)--w x
)--x y
...; <
FirstOrDefault..< J
(..J K
)..K L
;..L M
order00 
.00 
SetOrderStatus00 $
(00$ %
	Constants00% .
.00. /
ONORDER00/ 6
)006 7
;007 8
await11 +
_weavingOrderDocumentRepository11 5
.115 6
Update116 <
(11< =
order11= B
)11B C
;11C D
}22 
exsistingEstimation44 
.44  
Remove44  &
(44& '
)44' (
;44( )
await55 (
_estimationProductRepository55 .
.55. /
Update55/ 5
(555 6
exsistingEstimation556 I
)55I J
;55J K
_storage66 
.66 
Save66 
(66 
)66 
;66 
return88 
exsistingEstimation88 &
;88& '
}99 	
}:: 
};; ﬁ-
òD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Estimations\Productions\CommandHandlers\UpdateEstimationCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
Estimations# .
.. /
Productions/ :
.: ;
CommandHandlers; J
{ 
public 

class *
UpdateEstimationCommandHandler /
:0 1
ICommandHandler2 A
<A B*
UpdateEstimationProductCommandB `
,` a'
EstimatedProductionDocumentb }
>} ~
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly (
IEstimationProductRepository 5(
_estimationProductRepository6 R
;R S
private 
readonly )
IFabricConstructionRepository 6+
_constructionDocumentRepository7 V
;V W
private 
readonly +
IWeavingOrderDocumentRepository 8+
_weavingOrderDocumentRepository9 X
;X Y
public *
UpdateEstimationCommandHandler -
(- .
IStorage. 6
storage7 >
)> ?
{ 	
_storage 
= 
storage 
; (
_estimationProductRepository (
=) *
_storage+ 3
.3 4
GetRepository4 A
<A B(
IEstimationProductRepositoryB ^
>^ _
(_ `
)` a
;a b+
_constructionDocumentRepository +
=, -
_storage. 6
.6 7
GetRepository7 D
<D E)
IFabricConstructionRepositoryE b
>b c
(c d
)d e
;e f+
_weavingOrderDocumentRepository +
=, -
_storage. 6
.6 7
GetRepository7 D
<D E+
IWeavingOrderDocumentRepositoryE d
>d e
(e f
)f g
;g h
} 	
public!! 
async!! 
Task!! 
<!! '
EstimatedProductionDocument!! 5
>!!5 6
Handle!!7 =
(!!= >*
UpdateEstimationProductCommand!!> \
request!!] d
,!!d e
CancellationToken!!f w
cancellationToken	!!x â
)
!!â ä
{"" 	
var## 
query## 
=## (
_estimationProductRepository## 4
.##4 5
Query##5 :
.##: ;
Include##; B
(##B C
o##C D
=>##E G
o##H I
.##I J
EstimationProducts##J \
)##\ ]
;##] ^
var$$ 
exsistingEstimation$$ #
=$$$ %(
_estimationProductRepository$$& B
.$$B C
Find$$C G
($$G H
query$$H M
)$$M N
.$$N O
Where$$O T
($$T U
entity$$U [
=>$$\ ^
entity$$_ e
.$$e f
Identity$$f n
.$$n o
Equals$$o u
($$u v
request$$v }
.$$} ~
Id	$$~ Ä
)
$$Ä Å
)
$$Å Ç
.
$$Ç É
FirstOrDefault
$$É ë
(
$$ë í
)
$$í ì
;
$$ì î
if&& 
(&& 
exsistingEstimation&& "
==&&# %
null&&& *
)&&* +
{'' 
	Validator(( 
.(( 
ErrorValidation(( )
((() *
(((* +
$str((+ @
,((@ A
$str((B v
+((w x
request	((y Ä
.
((Ä Å
Id
((Å É
)
((É Ñ
)
((Ñ Ö
;
((Ö Ü
})) 
foreach++ 
(++ 
var++ 
product++ 
in++  "
exsistingEstimation++# 6
.++6 7
EstimationProducts++7 I
)++I J
{,, 
var-- 
requestProduct-- "
=--# $
request--% ,
.--, -
EstimationProducts--- ?
.--? @
Where--@ E
(--E F
e--F G
=>--H J
e--K L
.--L M
OrderNumber--M X
.--X Y
Equals--Y _
(--_ `
product--` g
.--g h
OrderDocument--h u
.--u v
Deserialize	--v Å
<
--Å Ç&
OrderDocumentValueObject
--Ç ö
>
--ö õ
(
--õ ú
)
--ú ù
.
--ù û
OrderNumber
--û ©
)
--© ™
)
--™ ´
.
--´ ¨
FirstOrDefault
--¨ ∫
(
--∫ ª
)
--ª º
;
--º Ω
var.. 
productGrade..  
=..! "
new..# &
ProductGrade..' 3
(..3 4
requestProduct..4 B
...B C
GradeA..C I
,..I J
requestProduct..K Y
...Y Z
GradeB..Z `
,..` a
requestProduct..b p
...p q
GradeC..q w
,..w x
requestProduct	..y á
.
..á à
GradeD
..à é
)
..é è
;
..è ê
product// 
.// 
SetProductGrade// '
(//' (
productGrade//( 4
)//4 5
;//5 6
product00 
.00 "
SetTotalGramEstimation00 .
(00. /
requestProduct00/ =
.00= >
TotalGramEstimation00> Q
)00Q R
;00R S
}11 
await33 (
_estimationProductRepository33 .
.33. /
Update33/ 5
(335 6
exsistingEstimation336 I
)33I J
;33J K
_storage44 
.44 
Save44 
(44 
)44 
;44 
return66 
exsistingEstimation66 &
;66& '
}77 	
}88 
}99 ™+
ïD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\FabricConstructions\CommandHandlers\PlaceConstructionCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
FabricConstructions# 6
.6 7
CommandHandlers7 F
{ 
public 

class +
PlaceConstructionCommandHandler 0
:1 2
ICommandHandler3 B
<B C(
AddFabricConstructionCommandC _
,_ `&
FabricConstructionDocumentC ]
>] ^
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly )
IFabricConstructionRepository 6+
_constructionDocumentRepository7 V
;V W
public +
PlaceConstructionCommandHandler .
(. /
IStorage/ 7
storage8 ?
)? @
{ 	
_storage 
= 
storage 
; +
_constructionDocumentRepository +
=, -
_storage. 6
.6 7
GetRepository7 D
<D E)
IFabricConstructionRepositoryE b
>b c
(c d
)d e
;e f
} 	
public 
async 
Task 
< &
FabricConstructionDocument 4
>4 5
Handle6 <
(< =(
AddFabricConstructionCommand= Y
requestZ a
,a b
CancellationToken7 H
cancellationTokenI Z
)Z [
{ 	
var '
exsistingConstructionNumber +
=, -+
_constructionDocumentRepository. M
.   
Find   
(   
construction   &
=>  ' )
construction  * 6
.  6 7
ConstructionNumber  7 I
.  I J
Equals  J P
(  P Q
request  Q X
.  X Y
ConstructionNumber  Y k
)  k l
&&  m o
construction!!* 6
.!!6 7
Deleted!!7 >
.!!> ?
Equals!!? E
(!!E F
false!!F K
)!!K L
)!!L M
."" 
Count"" 
("" 
)"" 
>"" 
$num""  
;""  !
if$$ 
($$ '
exsistingConstructionNumber$$ +
)$$+ ,
{%% 
	Validator&& 
.&& 
ErrorValidation&& )
(&&) *
(&&* +
$str&&+ ?
,&&? @
request&&A H
.&&H I
ConstructionNumber&&I [
+&&\ ]
$str&&^ o
)&&o p
)&&p q
;&&q r
}'' 
var))  
constructionDocument)) $
=))% &
new))' *&
FabricConstructionDocument))+ E
())E F
id))F H
:))H I
Guid))J N
.))N O
NewGuid))O V
())V W
)))W X
,))X Y
constructionNumber**@ R
:**R S
request**T [
.**[ \
ConstructionNumber**\ n
,**n o
amountOfWarp++@ L
:++L M
request++N U
.++U V
AmountOfWarp++V b
,++b c
amountOfWeft,,@ L
:,,L M
request,,N U
.,,U V
AmountOfWeft,,V b
,,,b c
width--@ E
:--E F
request--G N
.--N O
Width--O T
,--T U
	wofenType..@ I
:..I J
request..K R
...R S
	WovenType..S \
,..\ ]
warpType//@ H
://H I
request//J Q
.//Q R
WarpTypeForm//R ^
,//^ _
weftType00@ H
:00H I
request00J Q
.00Q R
WeftTypeForm00R ^
,00^ _
	totalYarn11@ I
:11I J
request11K R
.11R S
	TotalYarn11S \
,11\ ]
materialTypeName22@ P
:22P Q
request22R Y
.22Y Z
MaterialTypeName22Z j
)22j k
;22k l
if55 
(55 
request55 
.55 
	ItemsWarp55 !
.55! "
Count55" '
>55( )
$num55* +
)55+ ,
{66 
foreach77 
(77 
var77 
warp77 !
in77" $
request77% ,
.77, -
	ItemsWarp77- 6
)776 7
{88  
constructionDocument99 (
.99( )
AddWarp99) 0
(990 1
warp991 5
)995 6
;996 7
}:: 
};; 
if== 
(== 
request== 
.== 
	ItemsWeft== !
.==! "
Count==" '
>==( )
$num==* +
)==+ ,
{>> 
foreach?? 
(?? 
var?? 
weft?? !
in??" $
request??% ,
.??, -
	ItemsWeft??- 6
)??6 7
{@@  
constructionDocumentAA (
.AA( )
AddWeftAA) 0
(AA0 1
weftAA1 5
)AA5 6
;AA6 7
}BB 
}CC 
awaitFF +
_constructionDocumentRepositoryFF 1
.FF1 2
UpdateFF2 8
(FF8 9 
constructionDocumentFF9 M
)FFM N
;FFN O
_storageGG 
.GG 
SaveGG 
(GG 
)GG 
;GG 
returnII  
constructionDocumentII '
;II' (
}JJ 	
}KK 
}LL ›
ñD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\FabricConstructions\CommandHandlers\RemoveConstructionCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
FabricConstructions# 6
.6 7
CommandHandlers7 F
{ 
public 

class ,
 RemoveConstructionCommandHandler 1
:2 3
ICommandHandler4 C
<C D+
RemoveFabricConstructionCommandD c
,c d&
FabricConstructionDocumente 
>	 Ä
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly )
IFabricConstructionRepository 6+
_constructionDocumentRepository7 V
;V W
public ,
 RemoveConstructionCommandHandler /
(/ 0
IStorage0 8
storage9 @
)@ A
{ 	
_storage 
= 
storage 
; +
_constructionDocumentRepository +
=, -
_storage. 6
.6 7
GetRepository7 D
<D E)
IFabricConstructionRepositoryE b
>b c
(c d
)d e
;e f
} 	
public 
async 
Task 
< &
FabricConstructionDocument 4
>4 5
Handle6 <
(< =+
RemoveFabricConstructionCommand= \
request] d
,d e
CancellationToken7 H
cancellationTokenI Z
)Z [
{ 	
var  
constructionDocument $
=% &+
_constructionDocumentRepository' F
.F G
FindG K
(K L
entityL R
=>S U
entityV \
.\ ]
Identity] e
==f h
requesti p
.p q
Idq s
)s t
.F G
FirstOrDefaultG U
(U V
)V W
;W X
if 
(  
constructionDocument #
==$ &
null' +
)+ ,
{ 
throw   
	Validator   
.    
ErrorValidation    /
(  / 0
(  0 1
$str  1 5
,  5 6
$str  7 X
+  Y Z
request  [ b
.  b c
Id  c e
)  e f
)  f g
;  g h
}!!  
constructionDocument##  
.##  !
Remove##! '
(##' (
)##( )
;##) *
await$$ +
_constructionDocumentRepository$$ 1
.$$1 2
Update$$2 8
($$8 9 
constructionDocument$$9 M
)$$M N
;$$N O
_storage%% 
.%% 
Save%% 
(%% 
)%% 
;%% 
return''  
constructionDocument'' '
;''' (
}(( 	
})) 
}** í\
ñD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\FabricConstructions\CommandHandlers\UpdateConstructionCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
FabricConstructions# 6
.6 7
CommandHandlers7 F
{ 
public 

class ,
 UpdateConstructionCommandHandler 1
:2 3
ICommandHandler4 C
<C D+
UpdateFabricConstructionCommandD c
,c d&
FabricConstructionDocumente 
>	 Ä
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly )
IFabricConstructionRepository 6+
_constructionDocumentRepository7 V
;V W
private 
readonly #
IYarnDocumentRepository 0#
_yarnDocumentRepository1 H
;H I
public 
readonly #
IMaterialTypeRepository /#
_materialTypeRepository0 G
;G H
public 
readonly !
IYarnNumberRepository -!
_yarnNumberRepository. C
;C D
public ,
 UpdateConstructionCommandHandler /
(/ 0
IStorage0 8
storage9 @
)@ A
{ 	
_storage 
= 
storage 
; +
_constructionDocumentRepository +
=, -
_storage. 6
.6 7
GetRepository7 D
<D E)
IFabricConstructionRepositoryE b
>b c
(c d
)d e
;e f#
_yarnDocumentRepository #
=$ %
_storage& .
.. /
GetRepository/ <
<< =#
IYarnDocumentRepository= T
>T U
(U V
)V W
;W X#
_materialTypeRepository #
=$ %
_storage& .
.. /
GetRepository/ <
<< =#
IMaterialTypeRepository= T
>T U
(U V
)V W
;W X!
_yarnNumberRepository   !
=  " #
_storage  $ ,
.  , -
GetRepository  - :
<  : ;!
IYarnNumberRepository  ; P
>  P Q
(  Q R
)  R S
;  S T
}!! 	
public## 
async## 
Task## 
<## &
FabricConstructionDocument## 4
>##4 5
Handle##6 <
(##< =+
UpdateFabricConstructionCommand##= \
request##] d
,##d e
CancellationToken$$7 H
cancellationToken$$I Z
)$$Z [
{%% 	
var&& 
query&& 
=&& +
_constructionDocumentRepository&& 7
.&&7 8
Query&&8 =
;&&= >
var'' !
constructionDocuments'' %
=''& '+
_constructionDocumentRepository''( G
.''G H
Find''H L
(''L M
query''M R
)''R S
.''S T
Where''T Y
(''Y Z
Entity''Z `
=>''a c
Entity''d j
.''j k
Identity''k s
.''s t
Equals''t z
(''z {
request	''{ Ç
.
''Ç É
Id
''É Ö
)
''Ö Ü
)
''Ü á
.((G H
FirstOrDefault((H V
(((V W
)((W X
;((X Y
var** '
exsistingConstructionNumber** +
=**, -+
_constructionDocumentRepository**. M
.++ 
Find++ 
(++ 
construction++ &
=>++' )
construction++* 6
.++6 7
ConstructionNumber++7 I
.++I J
Equals++J P
(++P Q
request++Q X
.++X Y
ConstructionNumber++Y k
)++k l
&&++m o
construction,,* 6
.,,6 7
Deleted,,7 >
.,,> ?
Equals,,? E
(,,E F
false,,F K
),,K L
),,L M
.-- 
Count-- 
(-- 
)-- 
>-- 
$num--  
;--  !
if00 
(00 !
constructionDocuments00 %
==00& (
null00) -
)00- .
{11 
throw22 
	Validator22 
.22  
ErrorValidation22  /
(22/ 0
(220 1
$str221 5
,225 6
$str227 X
+22Y Z
request22[ b
.22b c
Id22c e
)22e f
)22f g
;22g h
}33 
if66 
(66 '
exsistingConstructionNumber66 +
&&66, .
!66/ 0!
constructionDocuments660 E
.66E F
Identity66F N
.66N O
Equals66O U
(66U V
request66V ]
.66] ^
Id66^ `
)66` a
)66a b
{77 
throw88 
	Validator88 
.88  
ErrorValidation88  /
(88/ 0
(880 1
$str881 E
,88E F
$str88G ]
+88^ _
request88` g
.88g h
ConstructionNumber88h z
+88{ |
$str	88} ç
)
88ç é
)
88é è
;
88è ê
}99 !
constructionDocuments;; !
.;;! "!
SetConstructionNumber;;" 7
(;;7 8
request;;8 ?
.;;? @
ConstructionNumber;;@ R
);;R S
;;;S T!
constructionDocuments<< !
.<<! "
SetAmountOfWarp<<" 1
(<<1 2
request<<2 9
.<<9 :
AmountOfWarp<<: F
)<<F G
;<<G H!
constructionDocuments== !
.==! "
SetAmountOfWeft==" 1
(==1 2
request==2 9
.==9 :
AmountOfWeft==: F
)==F G
;==G H!
constructionDocuments>> !
.>>! "
SetWidth>>" *
(>>* +
request>>+ 2
.>>2 3
Width>>3 8
)>>8 9
;>>9 :!
constructionDocuments?? !
.??! "
SetWovenType??" .
(??. /
request??/ 6
.??6 7
	WovenType??7 @
)??@ A
;??A B!
constructionDocuments@@ !
.@@! "
SetWarpType@@" -
(@@- .
request@@. 5
.@@5 6
WarpTypeForm@@6 B
)@@B C
;@@C D!
constructionDocumentsAA !
.AA! "
SetWeftTypeAA" -
(AA- .
requestAA. 5
.AA5 6
WeftTypeFormAA6 B
)AAB C
;AAC D!
constructionDocumentsBB !
.BB! "
SetTotalYarnBB" .
(BB. /
requestBB/ 6
.BB6 7
	TotalYarnBB7 @
)BB@ A
;BBA B!
constructionDocumentsCC !
.CC! "
SetMaterialTypeNameCC" 5
(CC5 6
requestCC6 =
.CC= >
MaterialTypeNameCC> N
)CCN O
;CCO P
foreachFF 
(FF 
varFF 
warpFF 
inFF  !
constructionDocumentsFF! 6
.FF6 7

ListOfWarpFF7 A
)FFA B
{GG 
varHH 
removedWarpHH 
=HH  !
requestHH" )
.HH) *
	ItemsWarpHH* 3
.HH3 4
WhereHH4 9
(HH9 :
oHH: ;
=>HH< >
oHH? @
.HH@ A
YarnIdHHA G
==HHH J
warpHHK O
.HHO P
YarnIdHHP V
)HHV W
.HHW X
FirstOrDefaultHHX f
(HHf g
)HHg h
;HHh i
ifJJ 
(JJ 
removedWarpJJ 
==JJ  "
nullJJ# '
)JJ' (
{KK !
constructionDocumentsLL )
.LL) *

RemoveWarpLL* 4
(LL4 5
warpLL5 9
)LL9 :
;LL: ;
}MM 
}NN 
foreachPP 
(PP 
varPP 
requestWarpPP $
inPP% '
requestPP( /
.PP/ 0
	ItemsWarpPP0 9
)PP9 :
{QQ 
varSS 
existingWarpSS  
=SS! "!
constructionDocumentsSS# 8
.SS8 9

ListOfWarpSS9 C
.SSC D
WhereSSD I
(SSI J
oSSJ K
=>SSL N
oSSO P
.SSP Q
YarnIdSSQ W
==SSX Z
requestWarpSS[ f
.SSf g
YarnIdSSg m
)SSm n
.SSn o
FirstOrDefaultSSo }
(SS} ~
)SS~ 
;	SS Ä
ifUU 
(UU 
existingWarpUU  
==UU! #
nullUU$ (
)UU( )
{VV !
constructionDocumentsXX )
.XX) *
AddWarpXX* 1
(XX1 2
requestWarpXX2 =
)XX= >
;XX> ?
}YY 
elseZZ 
{[[ 
if]] 
(]] 
existingWarp]] $
.]]$ %
YarnId]]% +
==]], .
requestWarp]]/ :
.]]: ;
YarnId]]; A
)]]A B
{^^ !
constructionDocuments__ -
.__- .

UpdateWarp__. 8
(__8 9
requestWarp__9 D
)__D E
;__E F
}`` 
}aa 
}bb 
foreachdd 
(dd 
vardd 
weftdd 
indd  !
constructionDocumentsdd! 6
.dd6 7

ListOfWeftdd7 A
)ddA B
{ee 
varff 
removedWeftff 
=ff  !
requestff" )
.ff) *
	ItemsWeftff* 3
.ff3 4
Whereff4 9
(ff9 :
off: ;
=>ff< >
off? @
.ff@ A
YarnIdffA G
==ffH J
weftffK O
.ffO P
YarnIdffP V
)ffV W
.ffW X
FirstOrDefaultffX f
(fff g
)ffg h
;ffh i
ifhh 
(hh 
removedWefthh 
==hh  "
nullhh# '
)hh' (
{ii !
constructionDocumentsjj )
.jj) *

RemoveWeftjj* 4
(jj4 5
weftjj5 9
)jj9 :
;jj: ;
}kk 
}ll 
foreachnn 
(nn 
varnn 
requestweftnn $
innn% '
requestnn( /
.nn/ 0
	ItemsWeftnn0 9
)nn9 :
{oo 
varpp 
existingWeftpp  
=pp! "!
constructionDocumentspp# 8
.pp8 9

ListOfWeftpp9 C
.ppC D
WhereppD I
(ppI J
oppJ K
=>ppL N
oppO P
.ppP Q
YarnIdppQ W
==ppX Z
requestweftpp[ f
.ppf g
YarnIdppg m
)ppm n
.ppn o
FirstOrDefaultppo }
(pp} ~
)pp~ 
;	pp Ä
ifrr 
(rr 
existingWeftrr  
==rr! #
nullrr$ (
)rr( )
{ss !
constructionDocumentsuu )
.uu) *
AddWeftuu* 1
(uu1 2
requestweftuu2 =
)uu= >
;uu> ?
}vv 
elseww 
{xx 
ifzz 
(zz 
existingWeftzz $
.zz$ %
YarnIdzz% +
==zz, .
requestweftzz/ :
.zz: ;
YarnIdzz; A
)zzA B
{{{ !
constructionDocuments|| -
.||- .

UpdateWeft||. 8
(||8 9
requestweft||9 D
)||D E
;||E F
}}} 
}~~ 
} 
await
ÅÅ -
_constructionDocumentRepository
ÅÅ 1
.
ÅÅ1 2
Update
ÅÅ2 8
(
ÅÅ8 9#
constructionDocuments
ÅÅ9 N
)
ÅÅN O
;
ÅÅO P
_storage
ÇÇ 
.
ÇÇ 
Save
ÇÇ 
(
ÇÇ 
)
ÇÇ 
;
ÇÇ 
return
ÑÑ #
constructionDocuments
ÑÑ (
;
ÑÑ( )
}
ÖÖ 	
}
ÜÜ 
}áá Ì
nD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Helpers\BeamMonitoringStatus.cs
	namespace 	
Manufactures
 
. 
Application "
." #
Helpers# *
{ 
public 

class  
BeamMonitoringStatus %
{ 
public 
static 
string 
	AVAILABLE &
=' (
$str) 4
;4 5
public 
static 
string 
USED !
=" #
$str$ *
;* +
public 
static 
string 
UNUSED #
=$ %
$str& .
;. /
} 
}		 ÷
cD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Helpers\Constants.cs
	namespace 	
Manufactures
 
. 
Application "
." #
Helpers# *
{ 
public 

class 
	Constants 
{ 
public 
static 
string 
PROCESS $
=% &
$str' 3
;3 4
public 
static 
string 
STOP !
=" #
$str$ *
;* +
public 
static 
string 
RESUME #
=$ %
$str& 0
;0 1
public		 
static		 
string		 
FINISH		 #
=		$ %
$str		& .
;		. /
public 
static 
string 
	AVAILABLE &
=' (
$str) 4
;4 5
public 
static 
string 
USED !
=" #
$str$ *
;* +
public 
static 
string 
UNUSED #
=$ %
$str& .
;. /
public 
static 
string 
WARP !
=" #
$str$ *
;* +
public 
static 
string 
WEFT !
=" #
$str$ +
;+ ,
public 
static 
string 
SPUN !
=" #
$str$ /
;/ 0
public 
static 
string 
OPENEND $
=% &
$str' 1
;1 2
public 
static 
string 
FILAMENT %
=& '
$str( 2
;2 3
public 
static 
string 
ALL  
=! "
$str# (
;( )
public 
static 
string 
ONORDER $
=% &
$str' 3
;3 4
public 
static 
string 
ONESTIMATED (
=) *
$str+ A
;A B
} 
} Ç
uD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Helpers\DailyOperationMachineStatus.cs
	namespace 	
Manufactures
 
. 
Application "
." #
Helpers# *
{ 
public 

class '
DailyOperationMachineStatus ,
{ 
public 
static 
string 
ONENTRY $
=% &
$str' .
;. /
public 
static 
string 
	ONPROCESS &
=' (
$str) 5
;5 6
public 
static 
string 
ONSTOP #
=$ %
$str& ,
;, -
public		 
static		 
string		 
ONRESUME		 %
=		& '
$str		( 2
;		2 3
public

 
static

 
string

 
ONFINISH

 %
=

& '
$str

( 0
;

0 1
} 
} ú
ñD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\MachinesPlanning\CommandHandlers\AddNewMachinePlanningCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
MachinesPlanning# 3
.3 4
CommandHandlers4 C
{ 
public 

class .
"AddNewEnginePlanningCommandHandler 3
: 	
ICommandHandler
 
< '
AddNewEnginePlanningCommand 5
,5 6$
MachinesPlanningDocument7 O
>O P
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly '
IMachinesPlanningRepository 4%
_enginePlanningRepository5 N
;N O
public .
"AddNewEnginePlanningCommandHandler 1
(1 2
IStorage2 :
storage; B
)B C
{ 	
_storage 
= 
storage 
; %
_enginePlanningRepository %
=& '
_storage( 0
.0 1
GetRepository1 >
<> ?'
IMachinesPlanningRepository? Z
>Z [
([ \
)\ ]
;] ^
} 	
public 
async 
Task 
< $
MachinesPlanningDocument 2
>2 3
Handle4 :
(: ;'
AddNewEnginePlanningCommand; V
requestW ^
,^ _
CancellationToken3 D
cancellationTokenE V
)V W
{ 	
var "
enginePlanningDocument &
=' (
new) ,$
MachinesPlanningDocument- E
(E F
GuidF J
.J K
NewGuidK R
(R S
)S T
,T U
requestD K
.K L
AreaL P
,P Q
requestD K
.K L
BlokL P
,P Q
requestD K
.K L

BlokKaizenL V
,V W
new  D G
UnitId  H N
(  N O
request  O V
.  V W
UnitDepartementId  W h
)  h i
,  i j
new!!D G
	MachineId!!H Q
(!!Q R
request!!R Y
.!!Y Z
	MachineId!!Z c
)!!c d
,!!d e
new""D G
UserId""H N
(""N O
request""O V
.""V W
UserMaintenanceId""W h
)""h i
,""i j
new##D G
UserId##H N
(##N O
request##O V
.##V W
UserOperatorId##W e
)##e f
)##f g
;##g h
await%% %
_enginePlanningRepository%% +
.%%+ ,
Update%%, 2
(%%2 3"
enginePlanningDocument%%3 I
)%%I J
;%%J K
_storage'' 
.'' 
Save'' 
('' 
)'' 
;'' 
return)) "
enginePlanningDocument)) )
;))) *
}** 	
}++ 
},, ∏
ñD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\MachinesPlanning\CommandHandlers\RemoveMachinePlanningCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
MachinesPlanning# 3
.3 4
CommandHandlers4 C
{ 
public 

class .
"RemoveEnginePlanningCommandHandler 3
: 	
ICommandHandler
 
< '
RemoveEnginePlanningCommand 5
,5 6$
MachinesPlanningDocument7 O
>O P
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly '
IMachinesPlanningRepository 4%
_enginePlanningRepository5 N
;N O
public .
"RemoveEnginePlanningCommandHandler 1
(1 2
IStorage2 :
storage; B
)B C
{ 	
_storage 
= 
storage 
; %
_enginePlanningRepository %
=& '
_storage( 0
.0 1
GetRepository1 >
<> ?'
IMachinesPlanningRepository? Z
>Z [
([ \
)\ ]
;] ^
} 	
public 
async 
Task 
< $
MachinesPlanningDocument 2
>2 3
Handle4 :
(: ;'
RemoveEnginePlanningCommand; V
requestW ^
,^ _
CancellationToken` q
cancellationToken	r É
)
É Ñ
{ 	
var "
enginePlanningDocument &
=' (%
_enginePlanningRepository) B
.B C
FindC G
(G H
oH I
=>J L
oM N
.N O
IdentityO W
==X Z
request[ b
.b c
Idc e
)e f
.f g
FirstOrDefaultg u
(u v
)v w
;w x
if 
( "
enginePlanningDocument &
==' )
null* .
). /
{ 
throw 
	Validator 
.  
ErrorValidation  /
(/ 0
(0 1
$str1 5
,5 6
$str7 Q
+R S
requestT [
.[ \
Id\ ^
)^ _
)_ `
;` a
}   "
enginePlanningDocument"" "
.""" #
Remove""# )
("") *
)""* +
;""+ ,
await$$ %
_enginePlanningRepository$$ +
.$$+ ,
Update$$, 2
($$2 3"
enginePlanningDocument$$3 I
)$$I J
;$$J K
_storage&& 
.&& 
Save&& 
(&& 
)&& 
;&& 
return(( "
enginePlanningDocument(( )
;(() *
})) 	
}** 
}++ „ 
ñD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\MachinesPlanning\CommandHandlers\UpdateMachinePlanningCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
MachinesPlanning# 3
.3 4
CommandHandlers4 C
{ 
public 

class .
"UpdateEnginePlanningCommandHandler 3
:4 5
ICommandHandler6 E
<E F'
UpdateEnginePlanningCommandF a
,a b$
MachinesPlanningDocumentc {
>{ |
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly '
IMachinesPlanningRepository 4%
_enginePlanningRepository5 N
;N O
public .
"UpdateEnginePlanningCommandHandler 1
(1 2
IStorage2 :
storage; B
)B C
{ 	
_storage 
= 
storage 
; %
_enginePlanningRepository %
=& '
_storage( 0
.0 1
GetRepository1 >
<> ?'
IMachinesPlanningRepository? Z
>Z [
([ \
)\ ]
;] ^
} 	
public 
async 
Task 
< $
MachinesPlanningDocument 2
>2 3
Handle4 :
(: ;'
UpdateEnginePlanningCommand; V
requestW ^
,^ _
CancellationToken` q
cancellationToken	r É
)
É Ñ
{ 	
var "
enginePlanningDocument &
=' (%
_enginePlanningRepository) B
.B C
FindC G
(G H
oH I
=>J L
oM N
.N O
IdentityO W
==X Z
request[ b
.b c
Idc e
)e f
.f g
FirstOrDefaultg u
(u v
)v w
;w x
if 
( "
enginePlanningDocument &
==' )
null* .
). /
{ 
throw   
	Validator   
.    
ErrorValidation    /
(  / 0
(  0 1
$str  1 5
,  5 6
$str  7 `
+  a b
request  c j
.  j k
Id  k m
)  m n
)  n o
;  o p
}!! "
enginePlanningDocument## "
.##" #

ChangeArea### -
(##- .
request##. 5
.##5 6
Area##6 :
)##: ;
;##; <"
enginePlanningDocument$$ "
.$$" #

ChangeBlok$$# -
($$- .
request$$. 5
.$$5 6
Blok$$6 :
)$$: ;
;$$; <"
enginePlanningDocument%% "
.%%" #
ChangeBlokKaizen%%# 3
(%%3 4
request%%4 ;
.%%; <

BlokKaizen%%< F
)%%F G
;%%G H"
enginePlanningDocument&& "
.&&" #
ChangeMachineId&&# 2
(&&2 3
request&&3 :
.&&: ;
	MachineId&&; D
)&&D E
;&&E F"
enginePlanningDocument'' "
.''" #"
ChangeUnitDepartmentId''# 9
(''9 :
request'': A
.''A B
UnitDepartementId''B S
)''S T
;''T U"
enginePlanningDocument(( "
.((" ##
ChangeUserMaintenanceId((# :
(((: ;
request((; B
.((B C
UserMaintenanceId((C T
)((T U
;((U V"
enginePlanningDocument)) "
.))" # 
ChangeUserOperatorId))# 7
())7 8
request))8 ?
.))? @
UserOperatorId))@ N
)))N O
;))O P
await++ %
_enginePlanningRepository++ +
.+++ ,
Update++, 2
(++2 3"
enginePlanningDocument++3 I
)++I J
;++J K
_storage-- 
.-- 
Save-- 
(-- 
)-- 
;-- 
return// "
enginePlanningDocument// )
;//) *
}00 	
}11 
}22 º
áD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Machines\CommandHandlers\AddNewMachineCommandHandlers.cs
	namespace 	
Manufactures
 
. 
Application "
." #
Machines# +
.+ ,
CommandHandlers, ;
{ 
public 

class (
AddNewMachineCommandHandlers -
:. /
ICommandHandler0 ?
<? @ 
AddNewMachineCommand@ T
,T U
MachineDocument@ O
>O P
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly 
IMachineRepository +
_machineRepository, >
;> ?
public (
AddNewMachineCommandHandlers +
(+ ,
IStorage, 4
storage5 <
)< =
{ 	
_storage 
= 
storage 
; 
_machineRepository 
=  
_storage! )
.) *
GetRepository* 7
<7 8
IMachineRepository8 J
>J K
(K L
)L M
;M N
} 	
public 
async 
Task 
< 
MachineDocument )
>) *
Handle+ 1
(1 2 
AddNewMachineCommand2 F
requestG N
,N O
CancellationTokenC T
cancellationTokenU f
)f g
{ 	
var 
exsistingMachine  
=! "
_machineRepository# 5
.5 6
Find6 :
(: ;
o; <
=>= ?
o@ A
.A B
MachineNumberB O
.O P
EqualsP V
(V W
requestW ^
.^ _
MachineNumber_ l
)l m
&&n p
o@ A
.A B
DeletedB I
.I J
ValueJ O
.O P
EqualsP V
(V W
falseW \
)\ ]
)] ^
.5 6
FirstOrDefault6 D
(D E
)E F
;F G
if"" 
("" 
exsistingMachine"" 
!=""  "
null""# '
)""' (
{## 
throw$$ 
	Validator$$ 
.$$  
ErrorValidation$$  /
($$/ 0
($$0 1
$str$$1 @
,$$@ A
$str$$B `
)$$` a
)$$a b
;$$b c
}%% 
var(( 
machineDocument(( 
=((  !
new((" %
MachineDocument((& 5
(((5 6
Guid((6 :
.((: ;
NewGuid((; B
(((B C
)((C D
,((D E
request))6 =
.))= >
MachineNumber))> K
,))K L
request**6 =
.**= >
Location**> F
,**F G
new++6 9
MachineTypeId++: G
(++G H
Guid++H L
.++L M
Parse++M R
(++R S
request++S Z
.++Z [
MachineTypeId++[ h
)++h i
)++i j
,++j k
new,,6 9
UnitId,,: @
(,,@ A
int,,A D
.,,D E
Parse,,E J
(,,J K
request,,K R
.,,R S
WeavingUnitId,,S `
),,` a
),,a b
),,b c
;,,c d
await.. 
_machineRepository.. $
...$ %
Update..% +
(..+ ,
machineDocument.., ;
)..; <
;..< =
_storage00 
.00 
Save00 
(00 
)00 
;00 
return22 
machineDocument22 "
;22" #
}33 	
}44 
}55 …
èD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Machines\CommandHandlers\RemoveExistingMachineCommandHandlers.cs
	namespace 	
Manufactures
 
. 
Application "
." #
Machines# +
.+ ,
CommandHandlers, ;
{ 
public 

class 0
$RemoveExistingMachineCommandHandlers 5
:6 7
ICommandHandler8 G
<G H(
RemoveExistingMachineCommandH d
,d e
MachineDocument@ O
>O P
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly 
IMachineRepository +
_machineRepository, >
;> ?
public 0
$RemoveExistingMachineCommandHandlers 3
(3 4
IStorage4 <
storage= D
)D E
{ 	
_storage 
= 
storage 
; 
_machineRepository 
=  
_storage! )
.) *
GetRepository* 7
<7 8
IMachineRepository8 J
>J K
(K L
)L M
;M N
} 	
public 
async 
Task 
< 
MachineDocument )
>) *
Handle+ 1
(1 2(
RemoveExistingMachineCommand2 N
requestO V
,V W
CancellationTokenX i
cancellationTokenj {
){ |
{ 	
var 
existingMachine 
=  !
_machineRepository" 4
.4 5
Find5 9
(9 :
o: ;
=>< >
o? @
.@ A
IdentityA I
==J L
requestM T
.T U
IdU W
)W X
.X Y
FirstOrDefaultY g
(g h
)h i
;i j
if 
( 
existingMachine 
==  "
null# '
)' (
{ 
throw 
	Validator 
.  
ErrorValidation  /
(/ 0
(0 1
$str1 5
,5 6
$str7 J
+K L
requestM T
.T U
IdU W
)W X
)X Y
;Y Z
}   
existingMachine"" 
."" 
Remove"" "
(""" #
)""# $
;""$ %
await$$ 
_machineRepository$$ $
.$$$ %
Update$$% +
($$+ ,
existingMachine$$, ;
)$$; <
;$$< =
_storage&& 
.&& 
Save&& 
(&& 
)&& 
;&& 
return(( 
existingMachine(( "
;((" #
})) 	
}** 
}++ °'
éD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Machines\CommandHandlers\UpdateExistingMachineCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
Machines# +
.+ ,
CommandHandlers, ;
{ 
public 

class /
#UpdateExistingMachineCommandHandler 4
:5 6
ICommandHandler7 F
<F G(
UpdateExistingMachineCommandG c
,c d
MachineDocument@ O
>O P
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly 
IMachineRepository +
_machineRepository, >
;> ?
public /
#UpdateExistingMachineCommandHandler 2
(2 3
IStorage3 ;
storage< C
)C D
{ 	
_storage 
= 
storage 
; 
_machineRepository 
=  
_storage! )
.) *
GetRepository* 7
<7 8
IMachineRepository8 J
>J K
(K L
)L M
;M N
} 	
public 
async 
Task 
< 
MachineDocument )
>) *
Handle+ 1
(1 2(
UpdateExistingMachineCommand2 N
requestO V
,V W
CancellationTokenX i
cancellationTokenj {
){ |
{ 	
var 
existingMachine 
=  !
_machineRepository" 4
.4 5
Find5 9
(9 :
o: ;
=>< >
o? @
.@ A
IdentityA I
==J L
requestM T
.T U
IdU W
)W X
.X Y
FirstOrDefaultY g
(g h
)h i
;i j
if 
( 
existingMachine 
==  "
null# '
)' (
{   
throw!! 
	Validator!! 
.!!  
ErrorValidation!!  /
(!!/ 0
(!!0 1
$str!!1 5
,!!5 6
$str!!7 J
+!!K L
request!!M T
.!!T U
Id!!U W
)!!W X
)!!X Y
;!!Y Z
}"" 
if$$ 
($$ 
existingMachine$$ 
.$$ 
MachineNumber$$ ,
!=$$- /
request$$0 7
.$$7 8
MachineNumber$$8 E
)$$E F
{%% 
var'' 

sameNumber'' 
=''  
_machineRepository''! 3
.''3 4
Find''4 8
(''8 9
o''9 :
=>''; =
o''> ?
.''? @
MachineNumber''@ M
.''M N
Equals''N T
(''T U
request''U \
.''\ ]
MachineNumber''] j
)''j k
&&''l n
o''o p
.''p q
Deleted''q x
.''x y
Value''y ~
==	'' Å
false
''Ç á
)
''á à
.
''à â
FirstOrDefault
''â ó
(
''ó ò
)
''ò ô
;
''ô ö
if)) 
()) 

sameNumber)) 
==))  
null))! %
)))% &
{** 
existingMachine++ #
.++# $
SetMachineNumber++$ 4
(++4 5
request++5 <
.++< =
MachineNumber++= J
)++J K
;++K L
},, 
else-- 
{.. 
throw// 
	Validator// #
.//# $
ErrorValidation//$ 3
(//3 4
(//4 5
$str//5 D
,//D E
$str//F d
)//d e
)//e f
;//f g
}00 
}11 
existingMachine44 
.44 
SetLocation44 '
(44' (
request44( /
.44/ 0
Location440 8
)448 9
;449 :
existingMachine55 
.55 
SetMachineTypeId55 ,
(55, -
new55- 0
MachineTypeId551 >
(55> ?
Guid55? C
.55C D
Parse55D I
(55I J
request55J Q
.55Q R
MachineTypeId55R _
)55_ `
)55` a
)55a b
;55b c
existingMachine66 
.66 
SetWeavingUnitId66 ,
(66, -
new66- 0
UnitId661 7
(667 8
int668 ;
.66; <
Parse66< A
(66A B
request66B I
.66I J
WeavingUnitId66J W
)66W X
)66X Y
)66Y Z
;66Z [
await88 
_machineRepository88 $
.88$ %
Update88% +
(88+ ,
existingMachine88, ;
)88; <
;88< =
_storage:: 
.:: 
Save:: 
(:: 
):: 
;:: 
return<< 
existingMachine<< "
;<<" #
}== 	
}>> 
}?? ˛
èD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\MachineTypes\CommandHandlers\AddNewMachineTypeCommandHandlers.cs
	namespace

 	
Manufactures


 
.

 
Application

 "
.

" #
MachineTypes

# /
.

/ 0
CommandHandlers

0 ?
{ 
public 

class ,
 AddNewMachineTypeCommandHandlers 1
:2 3
ICommandHandler4 C
<C D$
AddNewMachineTypeCommandD \
,\ ]
MachineTypeDocumentD W
>W X
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly "
IMachineTypeRepository /"
_machineTypeRepository0 F
;F G
public ,
 AddNewMachineTypeCommandHandlers /
(/ 0
IStorage0 8
storage9 @
)@ A
{ 	
_storage 
= 
storage 
; "
_machineTypeRepository "
=# $
_storage 
. 
GetRepository &
<& '"
IMachineTypeRepository' =
>= >
(> ?
)? @
;@ A
} 	
public 
async 
Task 
< 
MachineTypeDocument -
>- .
Handle/ 5
(5 6$
AddNewMachineTypeCommand6 N
requestO V
,V W
CancellationToken0 A
cancellationTokenB S
)S T
{ 	
var 
machineType 
= 
new !
MachineTypeDocument" 5
(5 6
Guid6 :
.: ;
NewGuid; B
(B C
)C D
,D E
request6 =
.= >
TypeName> F
,F G
request6 =
.= >
Speed> C
,C D
request6 =
.= >
MachineUnit> I
)I J
;J K
await!! "
_machineTypeRepository!! (
.!!( )
Update!!) /
(!!/ 0
machineType!!0 ;
)!!; <
;!!< =
_storage## 
.## 
Save## 
(## 
)## 
;## 
return%% 
machineType%% 
;%% 
}&& 	
}'' 
}(( ¢
óD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\MachineTypes\CommandHandlers\RemoveExistingMachineTypeCommandHandlers.cs
	namespace 	
Manufactures
 
. 
Application "
." #
MachineTypes# /
./ 0
CommandHandlers0 ?
{ 
public 

class 4
(RemoveExistingMachineTypeCommandHandlers 9
: 	
ICommandHandler
 
< ,
 RemoveExistingMachineTypeCommand :
,: ;
MachineTypeDocument< O
>O P
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly "
IMachineTypeRepository /"
_machineTypeRepository0 F
;F G
public 4
(RemoveExistingMachineTypeCommandHandlers 7
(7 8
IStorage8 @
storageA H
)H I
{ 	
_storage 
= 
storage 
; "
_machineTypeRepository "
=# $
_storage 
. 
GetRepository &
<& '"
IMachineTypeRepository' =
>= >
(> ?
)? @
;@ A
} 	
public 
async 
Task 
< 
MachineTypeDocument -
>- .
Handle/ 5
(5 6,
 RemoveExistingMachineTypeCommand6 V
requestW ^
,^ _
CancellationToken0 A
cancellationTokenB S
)S T
{ 	
var 
machineType 
= "
_machineTypeRepository &
.& '
Find' +
(+ ,
o, -
=>. 0
o1 2
.2 3
Identity3 ;
.; <
Equals< B
(B C
requestC J
.J K
IdK M
)M N
)N O
.& '
FirstOrDefault' 5
(5 6
)6 7
;7 8
if!! 
(!! 
machineType!! 
==!! 
null!! #
)!!# $
{"" 
throw## 
	Validator## 
.##  
ErrorValidation##  /
(##/ 0
(##0 1
$str##1 5
,##5 6
$str##7 W
+##X Y
request##Z a
.##a b
Id##b d
)##d e
)##e f
;##f g
}$$ 
machineType&& 
.&& 
Remove&& 
(&& 
)&&  
;&&  !
await(( "
_machineTypeRepository(( (
.((( )
Update(() /
(((/ 0
machineType((0 ;
)((; <
;((< =
_storage** 
.** 
Save** 
(** 
)** 
;** 
return-- 
machineType-- 
;-- 
}.. 	
}// 
}00 ∏
óD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\MachineTypes\CommandHandlers\UpdateExistingMachineTypeCommandHandlers.cs
	namespace 	
Manufactures
 
. 
Application "
." #
MachineTypes# /
./ 0
CommandHandlers0 ?
{ 
public 

class 4
(UpdateExistingMachineTypeCommandHandlers 9
: 	
ICommandHandler
 
< ,
 UpdateExistingMachineTypeCommand :
,: ;
MachineTypeDocument< O
>O P
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly "
IMachineTypeRepository /"
_machineTypeRepository0 F
;F G
public 4
(UpdateExistingMachineTypeCommandHandlers 7
(7 8
IStorage8 @
storageA H
)H I
{ 	
_storage 
= 
storage 
; "
_machineTypeRepository "
=# $
_storage 
. 
GetRepository &
<& '"
IMachineTypeRepository' =
>= >
(> ?
)? @
;@ A
} 	
public 
async 
Task 
< 
MachineTypeDocument -
>- .
Handle/ 5
(5 6,
 UpdateExistingMachineTypeCommand6 V
requestW ^
,^ _
CancellationToken0 A
cancellationTokenB S
)S T
{ 	
var 
machineType 
= "
_machineTypeRepository &
.& '
Find' +
(+ ,
o, -
=>. 0
o1 2
.2 3
Identity3 ;
.; <
Equals< B
(B C
requestC J
.J K
IdK M
)M N
)N O
.  & '
FirstOrDefault  ' 5
(  5 6
)  6 7
;  7 8
if"" 
("" 
machineType"" 
=="" 
null"" #
)""# $
{## 
throw$$ 
	Validator$$ 
.$$  
ErrorValidation$$  /
($$/ 0
($$0 1
$str$$1 5
,$$5 6
$str$$7 U
+$$V W
request$$X _
.$$_ `
Id$$` b
)$$b c
)$$c d
;$$d e
}%% 
machineType'' 
.'' 
SetTypeName'' #
(''# $
request''$ +
.''+ ,
TypeName'', 4
)''4 5
;''5 6
machineType(( 
.(( 
SetMachineSpeed(( '
(((' (
request((( /
.((/ 0
Speed((0 5
)((5 6
;((6 7
machineType)) 
.)) 
SetMachineUnit)) &
())& '
request))' .
.)). /
MachineUnit))/ :
))): ;
;)); <
await++ "
_machineTypeRepository++ (
.++( )
Update++) /
(++/ 0
machineType++0 ;
)++; <
;++< =
_storage-- 
.-- 
Save-- 
(-- 
)-- 
;-- 
return// 
machineType// 
;// 
}11 	
}22 
}33 Ÿ 
ãD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Materials\CommandHandlers\PlaceMaterialTypeCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
	Materials# ,
., -
CommandHandlers- <
{ 
public 

class +
PlaceMaterialTypeCommandHandler 0
:1 2
ICommandHandler3 B
<B C$
PlaceMaterialTypeCommandC [
,[ \ 
MaterialTypeDocument] q
>q r
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly #
IMaterialTypeRepository 0#
_materialTypeRepository1 H
;H I
public +
PlaceMaterialTypeCommandHandler .
(. /
IStorage/ 7
storage8 ?
)? @
{ 	
_storage 
= 
storage 
; #
_materialTypeRepository #
=$ %
storage& -
.- .
GetRepository. ;
<; <#
IMaterialTypeRepository< S
>S T
(T U
)U V
;V W
} 	
public 
async 
Task 
<  
MaterialTypeDocument .
>. /
Handle0 6
(6 7$
PlaceMaterialTypeCommand7 O
requestP W
,W X
CancellationToken/ @
cancellationTokenA R
)R S
{ 	
var !
exsistingMaterialCode %
=& '#
_materialTypeRepository( ?
.? @
Find@ D
(D E
materialE M
=>N P
materialQ Y
.Y Z
CodeZ ^
.^ _
Equals_ e
(e f
requestf m
.m n
Coden r
)r s
&&t v
materialQ Y
.Y Z
DeletedZ a
.a b
Equalsb h
(h i
falsei n
)n o
)o p
.p q
Countq v
(v w
)w x
>y z
$num{ |
;| }
if 
( !
exsistingMaterialCode %
)% &
{   
throw!! 
	Validator!! 
.!!  
ErrorValidation!!  /
(!!/ 0
(!!0 1
$str!!1 7
,!!7 8
$str!!9 E
+!!F G
request!!H O
.!!O P
Code!!P T
+!!U V
$str!!W g
)!!g h
)!!h i
;!!i j
}"" 
var$$ 
materialType$$ 
=$$ 
new$$ " 
MaterialTypeDocument$$# 7
($$7 8
id$$8 :
:$$: ;
Guid$$< @
.$$@ A
NewGuid$$A H
($$H I
)$$I J
,$$J K
code%%0 4
:%%4 5
request%%6 =
.%%= >
Code%%> B
,%%B C
name&&0 4
:&&4 5
request&&6 =
.&&= >
Name&&> B
,&&B C
description''0 ;
:''; <
request''= D
.''D E
Description''E P
)''P Q
;''Q R
if)) 
()) 
request)) 
.)) 
RingDocuments)) $
.))$ %
Count))% *
>))+ ,
$num))- .
))). /
{** 
foreach++ 
(++ 
var++ 
ringDocument++ (
in++) +
request++, 3
.++3 4
RingDocuments++4 A
)++A B
{,, 
materialType--  
.--  !
SetRingNumber--! .
(--. /
ringDocument--/ ;
)--; <
;--< =
}.. 
}// 
await11 #
_materialTypeRepository11 )
.11) *
Update11* 0
(110 1
materialType111 =
)11= >
;11> ?
_storage33 
.33 
Save33 
(33 
)33 
;33 
return55 
materialType55 
;55  
}66 	
}77 
}88 ‹
åD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Materials\CommandHandlers\RemoveMaterialTypeCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
	Materials# ,
., -
CommandHandlers- <
{ 
public 

class ,
 RemoveMaterialTypeCommandHandler 1
:2 3
ICommandHandler4 C
<C D%
RemoveMaterialTypeCommandD ]
,] ^ 
MaterialTypeDocument_ s
>s t
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly #
IMaterialTypeRepository 0#
_materialTypeRepository1 H
;H I
public ,
 RemoveMaterialTypeCommandHandler /
(/ 0
IStorage0 8
storage9 @
)@ A
{ 	
_storage 
= 
storage 
; #
_materialTypeRepository #
=$ %
_storage& .
.. /
GetRepository/ <
<< =#
IMaterialTypeRepository= T
>T U
(U V
)V W
;W X
} 	
public 
async 
Task 
<  
MaterialTypeDocument .
>. /
Handle0 6
(6 7%
RemoveMaterialTypeCommand7 P
requestQ X
,X Y
CancellationToken/ @
cancellationTokenA R
)R S
{ 	
var 
materialType 
= #
_materialTypeRepository 6
.6 7
Find7 ;
(; <
entity< B
=>C E
entityF L
.L M
IdentityM U
==V X
requestY `
.` a
Ida c
)c d
.6 7
FirstOrDefault7 E
(E F
)F G
;G H
if 
( 
materialType 
== 
null  $
)$ %
{ 
throw   
	Validator   
.    
ErrorValidation    /
(  / 0
(  0 1
$str  1 5
,  5 6
$str  7 H
+  I J
request  K R
.  R S
Id  S U
)  U V
)  V W
;  W X
}!! 
materialType## 
.## 
Remove## 
(##  
)##  !
;##! "
await%% #
_materialTypeRepository%% )
.%%) *
Update%%* 0
(%%0 1
materialType%%1 =
)%%= >
;%%> ?
_storage'' 
.'' 
Save'' 
('' 
)'' 
;'' 
return)) 
materialType)) 
;))  
}** 	
}++ 
},, †;
åD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Materials\CommandHandlers\UpdateMaterialTypeCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
	Materials# ,
., -
CommandHandlers- <
{ 
public 

class ,
 UpdateMaterialTypeCommandHandler 1
:2 3
ICommandHandler4 C
<C D%
UpdateMaterialTypeCommandD ]
,] ^ 
MaterialTypeDocument_ s
>s t
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly #
IMaterialTypeRepository 0#
_materialTypeRepository1 H
;H I
public ,
 UpdateMaterialTypeCommandHandler /
(/ 0
IStorage0 8
storage9 @
)@ A
{ 	
_storage 
= 
storage 
; #
_materialTypeRepository #
=$ %
_storage& .
.. /
GetRepository/ <
<< =#
IMaterialTypeRepository= T
>T U
(U V
)V W
;W X
} 	
public 
async 
Task 
<  
MaterialTypeDocument .
>. /
Handle0 6
(6 7%
UpdateMaterialTypeCommand7 P
requestQ X
,X Y
CancellationToken/ @
cancellationTokenA R
)R S
{ 	
var 
materialType 
= #
_materialTypeRepository 6
.6 7
Find7 ;
(; <
entity< B
=>C E
entityF L
.L M
IdentityM U
.U V
EqualsV \
(\ ]
request] d
.d e
Ide g
)g h
)h i
.6 7
FirstOrDefault7 E
(E F
)F G
;G H
var !
exsistingMaterialCode %
=& '#
_materialTypeRepository( ?
.? @
Find@ D
(D E
materialE M
=>N P
materialQ Y
.Y Z
CodeZ ^
.^ _
Equals_ e
(e f
requestf m
.m n
Coden r
)r s
&&t v
materialQ Y
.Y Z
DeletedZ a
.a b
Equalsb h
(h i
falsei n
)n o
)o p
.p q
Countq v
(v w
)w x
>y z
$num{ |
;| }
if"" 
("" 
materialType"" 
=="" 
null""  $
)""$ %
{## 
throw$$ 
	Validator$$ 
.$$  
ErrorValidation$$  /
($$/ 0
($$0 1
$str$$1 5
,$$5 6
$str$$7 H
+$$I J
request$$K R
.$$R S
Id$$S U
)$$U V
)$$V W
;$$W X
}%% 
if(( 
((( !
exsistingMaterialCode(( %
&&((& (
!(() *
materialType((* 6
.((6 7
Code((7 ;
.((; <
Equals((< B
(((B C
request((C J
.((J K
Code((K O
)((O P
)((P Q
{)) 
throw** 
	Validator** 
.**  
ErrorValidation**  /
(**/ 0
(**0 1
$str**1 7
,**7 8
$str**9 E
+**F G
request**H O
.**O P
Code**P T
+**U V
$str**W g
)**g h
)**h i
;**i j
}++ 
materialType-- 
.-- 
SetCode--  
(--  !
request--! (
.--( )
Code--) -
)--- .
;--. /
materialType.. 
... 
SetName..  
(..  !
request..! (
...( )
Name..) -
)..- .
;... /
materialType// 
.// 
SetDescription// '
(//' (
request//( /
./// 0
Description//0 ;
)//; <
;//< =
if11 
(11 
request11 
.11 
RingDocuments11 %
.11% &
Count11& +
>11, -
$num11. /
)11/ 0
{22 
foreach33 
(33 
var33 
exsistingRing33 )
in33* ,
materialType33- 9
.339 :
RingDocuments33: G
)33G H
{44 
var55 
requestRing55 #
=55$ %
request55& -
.55- .
RingDocuments55. ;
.55; <
Where55< A
(55A B
e55B C
=>55D F
e55G H
.55H I
Code55I M
.55M N
Equals55N T
(55T U
exsistingRing55U b
.55b c
Code55c g
)55g h
&&55i k
e55l m
.55m n
Number55n t
.55t u
Equals55u {
(55{ |
exsistingRing	55| â
.
55â ä
Number
55ä ê
)
55ê ë
)
55ë í
.
55í ì
FirstOrDefault
55ì °
(
55° ¢
)
55¢ £
;
55£ §
if77 
(77 
requestRing77 "
==77# %
null77& *
)77* +
{88 
materialType99 $
.99$ %
RemoveRingNumber99% 5
(995 6
exsistingRing996 C
)99C D
;99D E
}:: 
};; 
foreach== 
(== 
var== 
requestRing== (
in==) +
request==, 3
.==3 4
RingDocuments==4 A
)==A B
{>> 
var?? 
exsistingRing?? %
=??& '
materialType??( 4
.??4 5
RingDocuments??5 B
.??B C
Where??C H
(??H I
e??I J
=>??K M
e??N O
.??O P
Code??P T
.??T U
Equals??U [
(??[ \
requestRing??\ g
.??g h
Code??h l
)??l m
&&??n p
e??q r
.??r s
Number??s y
.??y z
Equals	??z Ä
(
??Ä Å
requestRing
??Å å
.
??å ç
Number
??ç ì
)
??ì î
)
??î ï
.
??ï ñ
FirstOrDefault
??ñ §
(
??§ •
)
??• ¶
;
??¶ ß
ifAA 
(AA 
exsistingRingAA %
==AA& (
nullAA) -
)AA- .
{BB 
materialTypeCC $
.CC$ %
SetRingNumberCC% 2
(CC2 3
requestRingCC3 >
)CC> ?
;CC? @
}DD 
}EE 
}FF 
elseFF 
{GG 
foreachHH 
(HH 
varHH 
exsistingRingHH *
inHH+ -
materialTypeHH. :
.HH: ;
RingDocumentsHH; H
)HHH I
{II 
materialTypeJJ  
.JJ  !
RemoveRingNumberJJ! 1
(JJ1 2
exsistingRingJJ2 ?
)JJ? @
;JJ@ A
}KK 
}LL 
awaitNN #
_materialTypeRepositoryNN )
.NN) *
UpdateNN* 0
(NN0 1
materialTypeNN1 =
)NN= >
;NN> ?
_storagePP 
.PP 
SavePP 
(PP 
)PP 
;PP 
returnRR 
materialTypeRR 
;RR  
}SS 	
}TT 
}UU ≈
ÖD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Operators\CommandHandlers\AddOperatorCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
	Operators# ,
., -
CommandHandlers- <
{ 
public 

class %
AddOperatorCommandHandler *
: 	
ICommandHandler
 
< 
AddOperatorCommand ,
,, -
OperatorDocument. >
>> ?
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly 
IOperatorRepository ,
_operatorRepository- @
;@ A
public %
AddOperatorCommandHandler (
(( )
IStorage) 1
storage2 9
)9 :
{ 	
_storage 
= 
storage 
; 
_operatorRepository 
=  !
_storage 
. 
GetRepository &
<& '
IOperatorRepository' :
>: ;
(; <
)< =
;= >
} 	
public 
async 
Task 
< 
OperatorDocument *
>* +
Handle, 2
(2 3
AddOperatorCommand3 E
requestF M
,M N
CancellationToken3 D
cancellationTokenE V
)V W
{ 	
var 
coreAccount 
= 
new 
CoreAccount 
(  
request  '
.' (
CoreAccount( 3
.3 4
MongoId4 ;
,; <
request  '
.' (
CoreAccount( 3
.3 4
Id4 6
.6 7
HasValue7 ?
?@ A
request  $ +
.  + ,
CoreAccount  , 7
.  7 8
Id  8 :
.  : ;
Value  ; @
:  A B
$num  C D
,  D E
request!!  '
.!!' (
CoreAccount!!( 3
.!!3 4
Name!!4 8
)!!8 9
;!!9 :
var"" 
newOperator"" 
="" 
new## 
OperatorDocument## $
(##$ %
Guid##% )
.##) *
NewGuid##* 1
(##1 2
)##2 3
,##3 4
coreAccount$$% 0
,$$0 1
request%%% ,
.%%, -
UnitId%%- 3
,%%3 4
request&&% ,
.&&, -
Group&&- 2
,&&2 3
request''% ,
.'', -

Assignment''- 7
,''7 8
request((% ,
.((, -
Type((- 1
)((1 2
;((2 3
await** 
_operatorRepository** %
.**% &
Update**& ,
(**, -
newOperator**- 8
)**8 9
;**9 :
_storage++ 
.++ 
Save++ 
(++ 
)++ 
;++ 
return-- 
newOperator-- 
;-- 
}.. 	
}// 
}00 „
àD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Operators\CommandHandlers\DeleteOperatorCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
	Operators# ,
., -
CommandHandlers- <
{ 
public 

class (
DeleteOperatorCommandHandler -
: 	
ICommandHandler
 
< !
RemoveOperatorCommand /
,/ 0
OperatorDocument1 A
>A B
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly 
IOperatorRepository ,
_operatorRepository- @
;@ A
public (
DeleteOperatorCommandHandler +
(+ ,
IStorage, 4
storage5 <
)< =
{ 	
_storage 
= 
storage 
; 
_operatorRepository 
=  !
_storage 
. 
GetRepository &
<& '
IOperatorRepository' :
>: ;
(; <
)< =
;= >
} 	
public 
async 
Task 
< 
OperatorDocument *
>* +
Handle, 2
(2 3!
RemoveOperatorCommand3 H
requestI P
,P Q
CancellationTokenR c
cancellationTokend u
)u v
{ 	
var 
existingOperator  
=! "
_operatorRepository #
.# $
Find$ (
(( )
o) *
=>+ -
o. /
./ 0
Identity0 8
.8 9
Equals9 ?
(? @
request@ G
.G H
IdH J
)J K
)K L
.# $
FirstOrDefault$ 2
(2 3
)3 4
;4 5
if   
(   
existingOperator    
==  ! #
null  $ (
)  ( )
{!! 
throw"" 
	Validator"" 
.""  
ErrorValidation""  /
(""/ 0
(""0 1
$str""1 5
,""5 6
$str""7 N
+""O P
request""Q X
.""X Y
Id""Y [
)""[ \
)""\ ]
;""] ^
}## 
existingOperator%% 
.%% 
Remove%% #
(%%# $
)%%$ %
;%%% &
await'' 
_operatorRepository'' %
.''% &
Update''& ,
('', -
existingOperator''- =
)''= >
;''> ?
_storage)) 
.)) 
Save)) 
()) 
))) 
;)) 
return++ 
existingOperator++ #
;++# $
},, 	
}-- 
}.. £
àD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Operators\CommandHandlers\UpdateOperatorCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
	Operators# ,
., -
CommandHandlers- <
{ 
public 

class (
UpdateOperatorCommandHandler -
: 	
ICommandHandler
 
< !
UpdateOperatorCommand /
,/ 0
OperatorDocument1 A
>A B
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly 
IOperatorRepository ,
_operatorRepository- @
;@ A
public (
UpdateOperatorCommandHandler +
(+ ,
IStorage, 4
storage5 <
)< =
{ 	
_storage 
= 
storage 
; 
_operatorRepository 
=  !
_storage 
. 
GetRepository &
<& '
IOperatorRepository' :
>: ;
(; <
)< =
;= >
} 	
public 
async 
Task 
< 
OperatorDocument *
>* +
Handle, 2
(2 3!
UpdateOperatorCommand3 H
requestI P
,P Q
CancellationToken3 D
cancellationTokenE V
)V W
{ 	
var 
coreAccount 
= 
new 
CoreAccount 
(  
request  '
.' (
CoreAccount( 3
.3 4
MongoId4 ;
,; <
request  '
.' (
CoreAccount( 3
.3 4
Id4 6
.6 7
HasValue7 ?
?@ A
request  $ +
.  + ,
CoreAccount  , 7
.  7 8
Id  8 :
.  : ;
Value  ; @
:  A B
$num  C D
,  D E
request!!  '
.!!' (
CoreAccount!!( 3
.!!3 4
Name!!4 8
)!!8 9
;!!9 :
var## 
existingOperator##  
=##! "
_operatorRepository$$ #
.$$# $
Find$$$ (
($$( )
o$$) *
=>$$+ -
o$$. /
.$$/ 0
Identity$$0 8
.$$8 9
Equals$$9 ?
($$? @
request$$@ G
.$$G H
Id$$H J
)$$J K
)$$K L
.%%# $
FirstOrDefault%%$ 2
(%%2 3
)%%3 4
;%%4 5
existingOperator'' 
.'' 
	SetUnitId'' &
(''& '
request''' .
.''. /
UnitId''/ 5
)''5 6
;''6 7
existingOperator(( 
.(( 
SetCoreAccount(( +
(((+ ,
coreAccount((, 7
)((7 8
;((8 9
existingOperator)) 
.)) 
SetAssignment)) *
())* +
request))+ 2
.))2 3

Assignment))3 =
)))= >
;))> ?
existingOperator** 
.** 
SetGroup** %
(**% &
request**& -
.**- .
Group**. 3
)**3 4
;**4 5
existingOperator++ 
.++ 
SetType++ $
(++$ %
request++% ,
.++, -
Type++- 1
)++1 2
;++2 3
await-- 
_operatorRepository-- %
.--% &
Update--& ,
(--, -
existingOperator--- =
)--= >
;--> ?
_storage.. 
... 
Save.. 
(.. 
).. 
;.. 
return00 
existingOperator00 #
;00# $
}11 	
}22 
}33 ≈ 
D:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Orders\CommandHandlers\AddOrderCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
Orders# )
.) *
CommandHandlers* 9
{ 
public 

class "
AddOrderCommandHandler '
:( )
ICommandHandler* 9
<9 :
PlaceOrderCommand: K
,K L
OrderDocumentM Z
>Z [
{ 
private 
readonly +
IWeavingOrderDocumentRepository 8+
_weavingOrderDocumentRepository9 X
;X Y
private 
readonly 
IStorage !
_storage" *
;* +
public "
AddOrderCommandHandler %
(% &
IStorage& .
storage/ 6
)6 7
{ 	
_storage 
= 
storage 
; +
_weavingOrderDocumentRepository +
=, -
_storage. 6
.6 7
GetRepository7 D
<D E+
IWeavingOrderDocumentRepositoryE d
>d e
(e f
)f g
;g h
} 	
public 
async 
Task 
< 
OrderDocument '
>' (
Handle) /
(/ 0
PlaceOrderCommand0 A
commandB I
,I J
CancellationToken7 H
cancellationTokenI Z
)Z [
{ 	
var 
orderNumber 
= 
await #+
_weavingOrderDocumentRepository$ C
.C D!
GetWeavingOrderNumberD Y
(Y Z
)Z [
;[ \
var   
orderStatus   
=   
	Constants   '
.  ' (
ONORDER  ( /
;  / 0
var"" 
order"" 
="" 
new"" 
OrderDocument"" )
("") *
id""* ,
:"", -
Guid"". 2
.""2 3
NewGuid""3 :
("": ;
)""; <
,""< =
orderNumber##1 <
:##< =
orderNumber##> I
,##I J
constructionId$$1 ?
:$$? @
new$$A D
ConstructionId$$E S
($$S T
Guid$$T X
.$$X Y
Parse$$Y ^
($$^ _
command$$_ f
.$$f g'
FabricConstructionDocument	$$g Å
.
$$Å Ç
Id
$$Ç Ñ
)
$$Ñ Ö
)
$$Ö Ü
,
$$Ü á
dateOrdered%%1 <
:%%< =
command%%> E
.%%E F
DateOrdered%%F Q
,%%Q R
period&&1 7
:&&7 8
command&&9 @
.&&@ A
Period&&A G
,&&G H
warpComposition''1 @
:''@ A
command''B I
.''I J
WarpComposition''J Y
,''Y Z
weftComposition((1 @
:((@ A
command((B I
.((I J
WeftComposition((J Y
,((Y Z

warpOrigin))1 ;
:)); <
command))= D
.))D E

WarpOrigin))E O
,))O P

weftOrigin**1 ;
:**; <
command**= D
.**D E

WeftOrigin**E O
,**O P

wholeGrade++1 ;
:++; <
command++= D
.++D E

WholeGrade++E O
,++O P
yarnType,,1 9
:,,9 :
command,,; B
.,,B C
YarnType,,C K
,,,K L
unitId--1 7
:--7 8
new--9 <
UnitId--= C
(--C D
command--D K
.--K L
WeavingUnit--L W
.--W X
Id--X Z
)--Z [
,--[ \
orderStatus..1 <
:..< =
orderStatus..> I
)..I J
;..J K
await00 +
_weavingOrderDocumentRepository00 1
.001 2
Update002 8
(008 9
order009 >
)00> ?
;00? @
_storage22 
.22 
Save22 
(22 
)22 
;22 
return44 
order44 
;44 
}55 	
}66 
}77 Å
ÖD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Orders\CommandHandlers\PlacedOrderCommandHandlerOld.cs
	namespace 	
Manufactures
 
. 
Domain 
. 
Orders $
.$ %
Commands% -
{		 
public

 

class

 $
PlaceOrderCommandHandler

 )
:

* +
ICommandHandler

, ;
<

; < 
PlaceOrderCommandOld

< P
,

P Q
ManufactureOrder

R b
>

b c
{ 
private 
readonly '
IManufactureOrderRepository 4
_orderRepository5 E
;E F
private 
readonly 
IStorage !
_storage" *
;* +
public $
PlaceOrderCommandHandler '
(' (
IStorage( 0
storage1 8
)8 9
{ 	
_orderRepository 
= 
storage &
.& '
GetRepository' 4
<4 5'
IManufactureOrderRepository5 P
>P Q
(Q R
)R S
;S T
_storage 
= 
storage 
; 
} 	
public 
async 
Task 
< 
ManufactureOrder *
>* +
Handle, 2
(2 3 
PlaceOrderCommandOld3 G
commandH O
,O P
CancellationTokenQ b
cancellationTokenc t
)t u
{ 	
var 
order 
= 
new 
ManufactureOrder ,
(, -
id- /
:/ 0
Guid1 5
.5 6
NewGuid6 =
(= >
)> ?
,? @
	orderDate 
: 
command "
." #
	OrderDate# ,
,, -
unitId 
: 
command 
.  
UnitDepartmentId  0
,0 1
	yarnCodes 
: 
command "
." #
	YarnCodes# ,
,, -
compositionId 
: 
command &
.& '
GoodsCompositionId' 9
,9 :
blended 
: 
command  
.  !
Blended! (
,( )
	machineId 
: 
command "
." #
	MachineId# ,
,, -
userId 
: 
command 
.  
UserId  &
)& '
;' (
await   
_orderRepository   "
.  " #
Update  # )
(  ) *
order  * /
)  / 0
;  0 1
_storage!! 
.!! 
Save!! 
(!! 
)!! 
;!! 
return## 
order## 
;## 
}$$ 	
}%% 
}&& ¯
ÖD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Orders\CommandHandlers\RemoveOrderCommandHandlerOld.cs
	namespace		 	
Manufactures		
 
.		 
Domain		 
.		 
Orders		 $
.		$ %
Commands		% -
{

 
public 

class (
RemoveOrderCommandHandlerOld -
:. /
ICommandHandler0 ?
<? @!
RemoveOrderCommandOld@ U
,U V
ManufactureOrderW g
>g h
{ 
private 
readonly '
IManufactureOrderRepository 4!
_manufactureOrderRepo5 J
;J K
public (
RemoveOrderCommandHandlerOld +
(+ ,
IStorage, 4
storage5 <
)< =
{ 	
Storage 
= 
storage 
; !
_manufactureOrderRepo !
=" #
Storage$ +
.+ ,
GetRepository, 9
<9 :'
IManufactureOrderRepository: U
>U V
(V W
)W X
;X Y
} 	
private 
IStorage 
Storage  
{! "
get# &
;& '
}( )
public 
async 
Task 
< 
ManufactureOrder *
>* +
Handle, 2
(2 3!
RemoveOrderCommandOld3 H
requestI P
,P Q
CancellationTokenR c
cancellationTokend u
)u v
{ 	
var 
order 
= !
_manufactureOrderRepo -
.- .
Find. 2
(2 3
o3 4
=>5 7
o8 9
.9 :
Identity: B
==C E
requestF M
.M N
IdN P
)P Q
.Q R
FirstOrDefaultR `
(` a
)a b
;b c
if 
( 
order 
== 
null 
) 
throw 
	Validator 
.  
ErrorValidation  /
(/ 0
(0 1
$str1 5
,5 6
$str7 H
+I J
requestK R
.R S
IdS U
)U V
)V W
;W X
order 
. 
Remove 
( 
) 
; 
await   !
_manufactureOrderRepo   '
.  ' (
Update  ( .
(  . /
order  / 4
)  4 5
;  5 6
Storage"" 
."" 
Save"" 
("" 
)"" 
;"" 
return$$ 
order$$ 
;$$ 
}%% 	
}&& 
}'' «
âD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Orders\CommandHandlers\RemoveWeavingOrderCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
Orders# )
.) *
CommandHandlers* 9
{ 
public 

class ,
 RemoveWeavingOrderCommandHandler 1
:2 3
ICommandHandler4 C
<C D
RemoveOrderCommandD V
,V W
OrderDocumentX e
>e f
{ 
private 
readonly +
IWeavingOrderDocumentRepository 8+
_weavingOrderDocumentRepository9 X
;X Y
private 
readonly 
IStorage !
_storage" *
;* +
public ,
 RemoveWeavingOrderCommandHandler /
(/ 0
IStorage0 8
storage9 @
)@ A
{ 	
_storage 
= 
storage 
; +
_weavingOrderDocumentRepository +
=, -
_storage. 6
.6 7
GetRepository7 D
<D E+
IWeavingOrderDocumentRepositoryE d
>d e
(e f
)f g
;g h
} 	
public 
async 
Task 
< 
OrderDocument '
>' (
Handle) /
(/ 0
RemoveOrderCommand0 B
commandC J
,J K
CancellationToken7 H
cancellationTokenI Z
)Z [
{ 	
var 
order 
= +
_weavingOrderDocumentRepository 7
.7 8
Find8 <
(< =
entity= C
=>D F
entityG M
.M N
IdentityN V
==W Y
commandZ a
.a b
Idb d
)d e
.7 8
FirstOrDefault8 F
(F G
)G H
;H I
if 
( 
order 
== 
null 
) 
{ 
throw   
	Validator   
.    
ErrorValidation    /
(  / 0
(  0 1
$str  1 5
,  5 6
$str  7 H
+  I J
command  K R
.  R S
Id  S U
)  U V
)  V W
;  W X
}!! 
order## 
.## 
Remove## 
(## 
)## 
;## 
await%% +
_weavingOrderDocumentRepository%% 1
.%%1 2
Update%%2 8
(%%8 9
order%%9 >
)%%> ?
;%%? @
_storage'' 
.'' 
Save'' 
('' 
)'' 
;'' 
return)) 
order)) 
;)) 
}** 	
}++ 
},, Ì#
ÇD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Orders\CommandHandlers\UpdateOrderCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
Orders# )
.) *
CommandHandlers* 9
{ 
public 

class %
UpdateOrderCommandHandler *
:+ ,
ICommandHandler- <
<< =
UpdateOrderCommand= O
,O P
OrderDocumentQ ^
>^ _
{ 
private 
readonly +
IWeavingOrderDocumentRepository 8+
_weavingOrderDocumentRepository9 X
;X Y
private 
readonly 
IStorage !
_storage" *
;* +
public %
UpdateOrderCommandHandler (
(( )
IStorage) 1
storage2 9
)9 :
{ 	
_storage 
= 
storage 
; +
_weavingOrderDocumentRepository +
=, -
_storage. 6
.6 7
GetRepository7 D
<D E+
IWeavingOrderDocumentRepositoryE d
>d e
(e f
)f g
;g h
} 	
public 
async 
Task 
< 
OrderDocument '
>' (
Handle) /
(/ 0
UpdateOrderCommand0 B
commandC J
,J K
CancellationToken7 H
cancellationTokenI Z
)Z [
{ 	
var 
order 
= +
_weavingOrderDocumentRepository 7
.7 8
Find8 <
(< =
entity= C
=>D F
entityG M
.M N
IdentityN V
==W Y
commandZ a
.a b
Idb d
)d e
.7 8
FirstOrDefault8 F
(F G
)G H
;H I
if!! 
(!! 
order!! 
==!! 
null!! 
)!! 
{"" 
throw## 
	Validator## 
.##  
ErrorValidation##  /
(##/ 0
(##0 1
$str##1 5
,##5 6
$str##7 H
+##I J
command##K R
.##R S
Id##S U
)##U V
)##V W
;##W X
}$$ 
order&& 
.&& )
SetFabricConstructionDocument&& /
(&&/ 0
new&&0 3
ConstructionId&&4 B
(&&B C
Guid&&C G
.&&G H
Parse&&H M
(&&M N
command&&N U
.&&U V&
FabricConstructionDocument&&V p
.&&p q
Id&&q s
)&&s t
)&&t u
)&&u v
;&&v w
order'' 
.'' 
SetWarpOrigin'' 
(''  
command''  '
.''' (

WarpOrigin''( 2
)''2 3
;''3 4
order(( 
.(( 
SetWeftOrigin(( 
(((  
command((  '
.((' (

WeftOrigin((( 2
)((2 3
;((3 4
order)) 
.)) 
SetWholeGrade)) 
())  
command))  '
.))' (

WholeGrade))( 2
)))2 3
;))3 4
order** 
.** 
SetYarnType** 
(** 
command** %
.**% &
YarnType**& .
)**. /
;**/ 0
order++ 
.++ 
	SetPeriod++ 
(++ 
command++ #
.++# $
Period++$ *
)++* +
;+++ ,
order,, 
.,, 
SetWarpComposition,, $
(,,$ %
command,,% ,
.,,, -
WarpComposition,,- <
),,< =
;,,= >
order-- 
.-- 
SetWeftComposition-- $
(--$ %
command--% ,
.--, -
WeftComposition--- <
)--< =
;--= >
order.. 
... 
SetWeavingUnit..  
(..  !
new..! $
UnitId..% +
(..+ ,
command.., 3
...3 4
WeavingUnit..4 ?
...? @
Id..@ B
)..B C
)..C D
;..D E
await00 +
_weavingOrderDocumentRepository00 1
.001 2
Update002 8
(008 9
order009 >
)00> ?
;00? @
_storage22 
.22 
Save22 
(22 
)22 
;22 
return44 
order44 
;44 
}55 	
}66 
}77 À
ÖD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Orders\CommandHandlers\UpdateOrderCommandHandlerOld.cs
	namespace		 	
Manufactures		
 
.		 
Domain		 
.		 
Orders		 $
.		$ %
Commands		% -
{

 
public 

class (
UpdateOrderCommandHandlerOld -
:. /
ICommandHandler0 ?
<? @!
UpdateOrderCommandOld@ U
,U V
ManufactureOrderW g
>g h
{ 
private 
readonly '
IManufactureOrderRepository 4!
_manufactureOrderRepo5 J
;J K
public (
UpdateOrderCommandHandlerOld +
(+ ,
IStorage, 4
storage5 <
)< =
{ 	
Storage 
= 
storage 
; !
_manufactureOrderRepo !
=" #
Storage$ +
.+ ,
GetRepository, 9
<9 :'
IManufactureOrderRepository: U
>U V
(V W
)W X
;X Y
} 	
private 
IStorage 
Storage  
{! "
get# &
;& '
}( )
public 
async 
Task 
< 
ManufactureOrder *
>* +
Handle, 2
(2 3!
UpdateOrderCommandOld3 H
requestI P
,P Q
CancellationTokenR c
cancellationTokend u
)u v
{ 	
var 
order 
= !
_manufactureOrderRepo -
.- .
Find. 2
(2 3
o3 4
=>5 7
o8 9
.9 :
Identity: B
==C E
requestF M
.M N
IdN P
)P Q
.Q R
FirstOrDefaultR `
(` a
)a b
;b c
if 
( 
order 
== 
null 
) 
throw 
	Validator 
.  
ErrorValidation  /
(/ 0
(0 1
$str1 5
,5 6
$str7 H
+I J
requestK R
.R S
IdS U
)U V
)V W
;W X
order 
. 

SetBlended 
( 
request $
.$ %
Blended% ,
), -
;- .
order   
.   
SetMachineId   
(   
request   &
.  & '
	MachineId  ' 0
)  0 1
;  1 2
order!! 
.!! 
SetUnitDepartment!! #
(!!# $
request!!$ +
.!!+ ,
UnitDepartmentId!!, <
)!!< =
;!!= >
order"" 
."" 
	SetUserId"" 
("" 
request"" #
.""# $
UserId""$ *
)""* +
;""+ ,
order## 
.## 
SetYarnCodes## 
(## 
request## &
.##& '
	YarnCodes##' 0
)##0 1
;##1 2
await%% !
_manufactureOrderRepo%% '
.%%' (
Update%%( .
(%%. /
order%%/ 4
)%%4 5
;%%5 6
Storage(( 
.(( 
Save(( 
((( 
)(( 
;(( 
return** 
order** 
;** 
}++ 	
},, 
}-- Ê
D:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Shifts\CommandHandlers\AddShiftCommandHandler.cs
	namespace

 	
Manufactures


 
.

 
Application

 "
.

" #
Shifts

# )
.

) *
CommandHandlers

* 9
{ 
public 

class "
AddShiftCommandHandler '
:( )
ICommandHandler* 9
<9 :
AddShiftCommand: I
,I J
ShiftDocumentK X
>X Y
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly 
IShiftRepository )
_shiftRepository* :
;: ;
public "
AddShiftCommandHandler %
(% &
IStorage& .
storage/ 6
)6 7
{ 	
_storage 
= 
storage 
; 
_shiftRepository 
= 
_storage 
. 
GetRepository &
<& '
IShiftRepository' 7
>7 8
(8 9
)9 :
;: ;
} 	
public 
async 
Task 
< 
ShiftDocument '
>' (
Handle) /
(/ 0
AddShiftCommand0 ?
request@ G
,G H
CancellationTokenI Z
cancellationToken[ l
)l m
{ 	
var 
	startTime 
= 
TimeSpan $
.$ %
Parse% *
(* +
request+ 2
.2 3
	StartTime3 <
)< =
;= >
var 
endTime 
= 
TimeSpan "
." #
Parse# (
(( )
request) 0
.0 1
EndTime1 8
)8 9
;9 :
var 
newShift 
= 
new 
ShiftDocument ,
(, -
Guid- 1
.1 2
NewGuid2 9
(9 :
): ;
,; <
request- 4
.4 5
Name5 9
,9 :
	startTime- 6
,6 7
endTime- 4
)4 5
;5 6
await!! 
_shiftRepository!! "
.!!" #
Update!!# )
(!!) *
newShift!!* 2
)!!2 3
;!!3 4
_storage"" 
."" 
Save"" 
("" 
)"" 
;"" 
return$$ 
newShift$$ 
;$$ 
}%% 	
}&& 
}'' ß
ÇD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Shifts\CommandHandlers\DeleteShiftCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
Shifts# )
.) *
CommandHandlers* 9
{ 
public 

class %
DeleteShiftCommandHandler *
:+ ,
ICommandHandler- <
<< =
RemoveShiftCommand= O
,O P
ShiftDocumentQ ^
>^ _
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly 
IShiftRepository )
_shiftRepository* :
;: ;
public %
DeleteShiftCommandHandler (
(( )
IStorage) 1
storage2 9
)9 :
{ 	
_storage 
= 
storage 
; 
_shiftRepository 
= 
_storage 
. 
GetRepository &
<& '
IShiftRepository' 7
>7 8
(8 9
)9 :
;: ;
} 	
public 
async 
Task 
< 
ShiftDocument '
>' (
Handle) /
(/ 0
RemoveShiftCommand0 B
requestC J
,J K
CancellationTokenL ]
cancellationToken^ o
)o p
{ 	
var 
existingShift 
= 
_shiftRepository  0
.0 1
Find1 5
(5 6
o6 7
=>8 :
o; <
.< =
Identity= E
.E F
EqualsF L
(L M
requestM T
.T U
IdU W
)W X
)X Y
.Y Z
FirstOrDefaultZ h
(h i
)i j
;j k
if 
( 
existingShift 
==  
null! %
)% &
{ 
throw 
	Validator 
.  
ErrorValidation  /
(/ 0
(0 1
$str1 5
,5 6
$str7 U
+V W
requestX _
._ `
Id` b
)b c
)c d
;d e
}   
existingShift"" 
."" 
Remove""  
(""  !
)""! "
;""" #
await$$ 
_shiftRepository$$ "
.$$" #
Update$$# )
($$) *
existingShift$$* 7
)$$7 8
;$$8 9
_storage&& 
.&& 
Save&& 
(&& 
)&& 
;&& 
return(( 
existingShift((  
;((  !
})) 	
}** 
}++ î
ÇD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Shifts\CommandHandlers\UpdateShiftCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
Shifts# )
.) *
CommandHandlers* 9
{ 
public 

class %
UpdateShiftCommandHandler *
:+ ,
ICommandHandler- <
<< =
UpdateShiftCommand= O
,O P
ShiftDocumentQ ^
>^ _
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly 
IShiftRepository )
_shiftRepository* :
;: ;
public %
UpdateShiftCommandHandler (
(( )
IStorage) 1
storage2 9
)9 :
{ 	
_storage 
= 
storage 
; 
_shiftRepository 
= 
_storage 
. 
GetRepository &
<& '
IShiftRepository' 7
>7 8
(8 9
)9 :
;: ;
} 	
public 
async 
Task 
< 
ShiftDocument '
>' (
Handle) /
(/ 0
UpdateShiftCommand0 B
requestC J
,J K
CancellationTokenL ]
cancellationToken^ o
)o p
{ 	
var 
existingShift 
= 
_shiftRepository  0
.0 1
Find1 5
(5 6
o6 7
=>8 :
o; <
.< =
Identity= E
.E F
EqualsF L
(L M
requestM T
.T U
IdU W
)W X
)X Y
.Y Z
FirstOrDefaultZ h
(h i
)i j
;j k
var 
	startTime 
= 
TimeSpan $
.$ %
Parse% *
(* +
request+ 2
.2 3
	StartTime3 <
)< =
;= >
var 
endTime 
= 
TimeSpan "
." #
Parse# (
(( )
request) 0
.0 1
EndTime1 8
)8 9
;9 :
if   
(   
existingShift   
==    
null  ! %
)  % &
{!! 
throw"" 
	Validator"" 
.""  
ErrorValidation""  /
(""/ 0
(""0 1
$str""1 5
,""5 6
$str""7 U
+""V W
request""X _
.""_ `
Id""` b
)""b c
)""c d
;""d e
}## 
existingShift%% 
.%% 
SetName%% !
(%%! "
request%%" )
.%%) *
Name%%* .
)%%. /
;%%/ 0
existingShift&& 
.&& 
SetStartTime&& &
(&&& '
	startTime&&' 0
)&&0 1
;&&1 2
existingShift'' 
.'' 

SetEndTime'' $
(''$ %
endTime''% ,
)'', -
;''- .
await)) 
_shiftRepository)) "
.))" #
Update))# )
())) *
existingShift))* 7
)))7 8
;))8 9
_storage** 
.** 
Save** 
(** 
)** 
;** 
return,, 
existingShift,,  
;,,  !
}-- 	
}.. 
}// ¶
ãD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Suppliers\CommandHandlers\CreateNewSupplierCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
	Suppliers# ,
., -
CommandHandlers- <
{ 
public 

class +
CreateNewSupplierCommandHandler 0
:1 2
ICommandHandler3 B
<B C#
PlaceNewSupplierCommandC Z
,Z [#
WeavingSupplierDocument\ s
>s t
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly &
IWeavingSupplierRepository 3&
_weavingSupplierRepository4 N
;N O
public +
CreateNewSupplierCommandHandler .
(. /
IStorage/ 7
storage8 ?
)? @
{ 	
_storage 
= 
storage 
; &
_weavingSupplierRepository &
=' (
_storage) 1
.1 2
GetRepository2 ?
<? @&
IWeavingSupplierRepository@ Z
>Z [
([ \
)\ ]
;] ^
} 	
public 
async 
Task 
< #
WeavingSupplierDocument 1
>1 2
Handle3 9
(9 :#
PlaceNewSupplierCommand: Q
requestR Y
,Y Z
CancellationToken[ l
cancellationTokenm ~
)~ 
{ 	
var 
hasSupplier 
= &
_weavingSupplierRepository 8
.8 9
Find9 =
(= >
supplier> F
=>G I
supplierJ R
.R S
CodeS W
.W X
EqualsX ^
(^ _
request_ f
.f g
Codeg k
)k l
&&m o
supplierJ R
.R S
DeletedS Z
.Z [
Equals[ a
(a b
falseb g
)g h
)h i
.i j
Countj o
(o p
)p q
>=r t
$numu v
;v w
if 
( 
hasSupplier 
) 
{   
throw!! 
	Validator!! 
.!!  
ErrorValidation!!  /
(!!/ 0
(!!0 1
$str!!1 E
,!!E F
request!!G N
.!!N O
Code!!O S
+!!T U
$str!!V f
)!!f g
)!!g h
;!!h i
}"" 
var$$ 
supplierDocument$$  
=$$! "
new$$# &#
WeavingSupplierDocument$$' >
($$> ?
Guid$$? C
.$$C D
NewGuid$$D K
($$K L
)$$L M
,$$M N
request$$O V
.$$V W
Code$$W [
,$$[ \
request$$] d
.$$d e
Name$$e i
,$$i j
request$$k r
.$$r s
CoreSupplierId	$$s Å
)
$$Å Ç
;
$$Ç É
await&& &
_weavingSupplierRepository&& ,
.&&, -
Update&&- 3
(&&3 4
supplierDocument&&4 D
)&&D E
;&&E F
_storage'' 
.'' 
Save'' 
('' 
)'' 
;'' 
return)) 
supplierDocument)) #
;))# $
}** 	
}++ 
},, À
ëD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Suppliers\CommandHandlers\RemoveAvailableSupplierCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
	Suppliers# ,
., -
CommandHandlers- <
{ 
public 

class 1
%RemoveAvailableSupplierCommandHandler 6
:7 8
ICommandHandler9 H
<H I!
RemoveSupplierCommandI ^
,^ _#
WeavingSupplierDocument` w
>w x
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly &
IWeavingSupplierRepository 3&
_weavingSupplierRepository4 N
;N O
public 1
%RemoveAvailableSupplierCommandHandler 4
(4 5
IStorage5 =
storage> E
)E F
{ 	
_storage 
= 
storage 
; &
_weavingSupplierRepository &
=' (
_storage) 1
.1 2
GetRepository2 ?
<? @&
IWeavingSupplierRepository@ Z
>Z [
([ \
)\ ]
;] ^
} 	
public 
async 
Task 
< #
WeavingSupplierDocument 1
>1 2
Handle3 9
(9 :!
RemoveSupplierCommand: O
requestP W
,W X
CancellationTokenY j
cancellationTokenk |
)| }
{ 	
var 
supplierDocument  
=! "&
_weavingSupplierRepository# =
.= >
Find> B
(B C
supplierC K
=>L N
supplierO W
.W X
IdentityX `
.` a
Equalsa g
(g h
requesth o
.o p
Idp r
)r s
)s t
.t u
FirstOrDefault	u É
(
É Ñ
)
Ñ Ö
;
Ö Ü
if 
( 
supplierDocument  
==! #
null$ (
)( )
{ 
throw 
	Validator 
.  
ErrorValidation  /
(/ 0
(0 1
$str1 5
,5 6
$str7 N
+O P
requestQ X
.X Y
IdY [
)[ \
)\ ]
;] ^
} 
supplierDocument!! 
.!! 
Remove!! #
(!!# $
)!!$ %
;!!% &
await"" &
_weavingSupplierRepository"" ,
."", -
Update""- 3
(""3 4
supplierDocument""4 D
)""D E
;""E F
_storage## 
.## 
Save## 
(## 
)## 
;## 
return%% 
supplierDocument%% #
;%%# $
}&& 	
}'' 
}(( ‚$
ëD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Suppliers\CommandHandlers\UpdateAvailableSupplierCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
	Suppliers# ,
., -
CommandHandlers- <
{ 
class 	1
%UpdateAvailableSupplierCommandHandler
 /
:0 1
ICommandHandler2 A
<A B*
UpdateExsistingSupplierCommandB `
,` a#
WeavingSupplierDocumentb y
>y z
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly &
IWeavingSupplierRepository 3&
_weavingSupplierRepository4 N
;N O
public 1
%UpdateAvailableSupplierCommandHandler 4
(4 5
IStorage5 =
storage> E
)E F
{ 	
_storage 
= 
storage 
; &
_weavingSupplierRepository &
=' (
_storage) 1
.1 2
GetRepository2 ?
<? @&
IWeavingSupplierRepository@ Z
>Z [
([ \
)\ ]
;] ^
} 	
public 
async 
Task 
< #
WeavingSupplierDocument 1
>1 2
Handle3 9
(9 :*
UpdateExsistingSupplierCommand: X
requestY `
,` a
CancellationTokenb s
cancellationToken	t Ö
)
Ö Ü
{ 	
var 
supplierDocument  
=! "&
_weavingSupplierRepository# =
.= >
Find> B
(B C
supplierC K
=>L N
supplierO W
.W X
IdentityX `
.` a
Equalsa g
(g h
requesth o
.o p
Idp r
)r s
)s t
.t u
FirstOrDefault	u É
(
É Ñ
)
Ñ Ö
;
Ö Ü
var 
hasExsistingCode  
=! "&
_weavingSupplierRepository# =
.= >
Find> B
(B C
supplierC K
=>L N
supplierO W
.W X
CodeX \
.\ ]
Equals] c
(c d
requestd k
.k l
Codel p
)p q
&&r t
supplierO W
.W X
DeletedX _
._ `
Equals` f
(f g
falseg l
)l m
)m n
.n o
Counto t
(t u
)u v
>=w y
$numz {
;{ |
if 
( 
supplierDocument  
==! #
null$ (
)( )
{ 
throw   
	Validator   
.    
ErrorValidation    /
(  / 0
(  0 1
$str  1 5
,  5 6
$str  7 H
+  I J
request  K R
.  R S
Id  S U
)  U V
)  V W
;  W X
}!! 
if$$ 
($$ 
hasExsistingCode$$ 
&&$$  "
!$$# $
supplierDocument$$$ 4
.$$4 5
Code$$5 9
.$$9 :
Equals$$: @
($$@ A
request$$A H
.$$H I
Code$$I M
)$$M N
)$$N O
{%% 
throw&& 
	Validator&& 
.&&  
ErrorValidation&&  /
(&&/ 0
(&&0 1
$str&&1 7
,&&7 8
$str&&9 F
+&&G H
request&&I P
.&&P Q
Code&&Q U
+&&V W
$str&&X h
)&&h i
)&&i j
;&&j k
}'' 
supplierDocument)) 
.)) 
SetCode)) $
())$ %
request))% ,
.)), -
Code))- 1
)))1 2
;))2 3
supplierDocument** 
.** 
SetName** $
(**$ %
request**% ,
.**, -
Name**- 1
)**1 2
;**2 3
supplierDocument++ 
.++ 
SetCoreSupplierId++ .
(++. /
request++/ 6
.++6 7
CoreSupplierId++7 E
)++E F
;++F G
await-- &
_weavingSupplierRepository-- ,
.--, -
Update--- 3
(--3 4
supplierDocument--4 D
)--D E
;--E F
_storage.. 
... 
Save.. 
(.. 
).. 
;.. 
return00 
supplierDocument00 #
;00# $
}11 	
}22 
}33 ˚
åD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\YarnNumbers\CommandHandlers\AddNewYarnNumberCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
YarnNumbers# .
.. /
CommandHandlers/ >
{ 
public 

class *
AddNewYarnNumberCommandHandler /
:0 1
ICommandHandler2 A
<A B#
AddNewYarnNumberCommandB Y
,Y Z
YarnNumberDocument[ m
>m n
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly !
IYarnNumberRepository .!
_YarnNumberRepository/ D
;D E
public *
AddNewYarnNumberCommandHandler -
(- .
IStorage. 6
storage7 >
)> ?
{ 	
_storage 
= 
storage 
; !
_YarnNumberRepository !
=" #
_storage$ ,
., -
GetRepository- :
<: ;!
IYarnNumberRepository; P
>P Q
(Q R
)R S
;S T
} 	
public 
async 
Task 
< 
YarnNumberDocument ,
>, -
Handle. 4
(4 5#
AddNewYarnNumberCommand5 L
requestM T
,T U
CancellationToken/ @
cancellationTokenA R
)R S
{ 	
var 
existingYarnNumber "
=# $!
_YarnNumberRepository %
.% &
Find& *
(* +

yarnNumber+ 5
=>6 8

yarnNumber9 C
.C D
RingTypeD L
==M O
requestP W
.W X
RingTypeX `
&&a c

yarnNumber9 C
.C D
NumberD J
==K M
requestN U
.U V
NumberV \
)\ ]
.] ^
FirstOrDefault^ l
(l m
)m n
;n o
if!! 
(!! 
existingYarnNumber!! !
!=!!" $
null!!% )
)!!) *
{"" 
throw## 
	Validator## 
.##  
ErrorValidation##  /
(##/ 0
(##0 1
$str##1 ;
,##; <
$str##= b
)##b c
,##c d
($$0 1
$str$$1 9
,$$9 :
$str$$; `
)$$` a
)$$a b
;$$b c
}%% 
var'' 
yarnNumberDocument'' "
=''# $
new''% (
YarnNumberDocument'') ;
(''; <
identity''< D
:''D E
Guid''F J
.''J K
NewGuid''K R
(''R S
)''S T
,''T U
code((0 4
:((4 5
request((6 =
.((= >
Code((> B
,((B C
number))0 6
:))6 7
request))8 ?
.))? @
Number))@ F
,))F G
ringType**0 8
:**8 9
request**: A
.**A B
RingType**B J
,**J K
description++0 ;
:++; <
request++= D
.++D E
Description++E P
)++P Q
;++Q R
await-- !
_YarnNumberRepository-- '
.--' (
Update--( .
(--. /
yarnNumberDocument--/ A
)--A B
;--B C
_storage// 
.// 
Save// 
(// 
)// 
;// 
return11 
yarnNumberDocument11 %
;11% &
}22 	
}33 
}44 ‰
åD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\YarnNumbers\CommandHandlers\RemoveYarnNumberCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
YarnNumbers# .
.. /
CommandHandlers/ >
{ 
public 

class *
RemoveYarnNumberCommandHandler /
:0 1
ICommandHandler2 A
<A B#
RemoveYarnNumberCommandB Y
,Y Z
YarnNumberDocument[ m
>m n
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly !
IYarnNumberRepository .!
_YarnNumberRepository/ D
;D E
public *
RemoveYarnNumberCommandHandler -
(- .
IStorage. 6
storage7 >
)> ?
{ 	
_storage 
= 
storage 
; !
_YarnNumberRepository !
=" #
_storage$ ,
., -
GetRepository- :
<: ;!
IYarnNumberRepository; P
>P Q
(Q R
)R S
;S T
} 	
public 
async 
Task 
< 
YarnNumberDocument ,
>, -
Handle. 4
(4 5#
RemoveYarnNumberCommand5 L
requestM T
,T U
CancellationTokenV g
cancellationTokenh y
)y z
{ 	
var 
yarnNumberDocument "
=# $!
_YarnNumberRepository% :
.: ;
Find; ?
(? @
entity@ F
=>G I
entityJ P
.P Q
IdentityQ Y
==Z \
request] d
.d e
Ide g
)g h
.h i
FirstOrDefaulti w
(w x
)x y
;y z
if 
( 
yarnNumberDocument "
==# %
null& *
)* +
{ 
throw 
	Validator 
.  
ErrorValidation  /
(/ 0
(0 1
$str1 5
,5 6
$str7 J
+K L
requestM T
.T U
IdU W
)W X
)X Y
;Y Z
} 
yarnNumberDocument!! 
.!! 
Remove!! %
(!!% &
)!!& '
;!!' (
await## !
_YarnNumberRepository## '
.##' (
Update##( .
(##. /
yarnNumberDocument##/ A
)##A B
;##B C
_storage%% 
.%% 
Save%% 
(%% 
)%% 
;%% 
return'' 
yarnNumberDocument'' %
;''% &
}(( 	
})) 
}** Ì1
åD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\YarnNumbers\CommandHandlers\UpdateYarnNumberCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
YarnNumbers# .
.. /
CommandHandlers/ >
{ 
public 

class *
UpdateYarnNumberCommandHandler /
:0 1
ICommandHandler2 A
<A B#
UpdateYarnNumberCommandB Y
,Y Z
YarnNumberDocument[ m
>m n
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly !
IYarnNumberRepository .!
_YarnNumberRepository/ D
;D E
public *
UpdateYarnNumberCommandHandler -
(- .
IStorage. 6
storage7 >
)> ?
{ 	
_storage 
= 
storage 
; !
_YarnNumberRepository !
=" #
storage$ +
.+ ,
GetRepository, 9
<9 :!
IYarnNumberRepository: O
>O P
(P Q
)Q R
;R S
} 	
public 
async 
Task 
< 
YarnNumberDocument ,
>, -
Handle. 4
(4 5#
UpdateYarnNumberCommand5 L
requestM T
,T U
CancellationToken/ @
cancellationTokenA R
)R S
{ 	
var 
yarnNumberDocument "
=# $!
_YarnNumberRepository %
.% &
Find& *
(* +
entity+ 1
=>2 4
entity5 ;
.; <
Identity< D
.D E
EqualsE K
(K L
requestL S
.S T
IdT V
)V W
)W X
.> ?
FirstOrDefault? M
(M N
)N O
;O P
if 
( 
yarnNumberDocument !
==" $
null% )
)) *
{   
throw!! 
	Validator!! 
.!!  
ErrorValidation!!  /
(!!/ 0
(!!0 1
$str!!1 5
,!!5 6
$str!!7 Q
+!!R S
request!!T [
.!![ \
Id!!\ ^
)!!^ _
)!!_ `
;!!` a
}"" 
if$$ 
($$ 
yarnNumberDocument$$ !
.$$! "
RingType$$" *
!=$$+ -
request$$. 5
.$$5 6
RingType$$6 >
)$$? @
{%% 
var&& 
existingYarnNumbers&& '
=&&( )!
_YarnNumberRepository&&* ?
.&&? @
Find&&@ D
(&&D E
entity&&E K
=>&&L N
entity&&O U
.&&U V
RingType&&V ^
.&&^ _
Equals&&_ e
(&&e f
request&&f m
.&&m n
RingType&&n v
)&&v w
)&&w x
.&&x y
ToList&&y 
(	&& Ä
)
&&Ä Å
;
&&Å Ç
foreach(( 
((( 
var(( 

yarnNumber(( &
in((' )
existingYarnNumbers((* =
)((= >
{)) 
if** 
(** 

yarnNumber** !
.**! "
RingType**" *
==**+ -
request**. 5
.**5 6
RingType**6 >
&&**? A

yarnNumber**B L
.**L M
Number**M S
==**T V
request**W ^
.**^ _
Number**_ e
)**e f
{++ 
throw,, 
	Validator,, '
.,,' (
ErrorValidation,,( 7
(,,7 8
(,,8 9
$str,,9 C
,,,C D
$str,,E j
),,j k
),,k l
;,,l m
}-- 
}.. 
}// 
if11 
(11 
yarnNumberDocument11 !
.11! "
Number11" (
!=11) +
request11, 3
.113 4
Number114 :
)11: ;
{22 
var33 
existingYarnNumbers33 '
=33( )!
_YarnNumberRepository33* ?
.33? @
Find33@ D
(33D E
entity33E K
=>33L N
entity33O U
.33U V
Number33V \
.33\ ]
Equals33] c
(33c d
request33d k
.33k l
Number33l r
)33r s
)33s t
.33t u
ToList33u {
(33{ |
)33| }
;33} ~
foreach55 
(55 
var55 

yarnNumber55 '
in55( *
existingYarnNumbers55+ >
)55> ?
{66 
if77 
(77 

yarnNumber77 "
.77" #
RingType77# +
==77, .
request77/ 6
.776 7
RingType777 ?
&&77@ B

yarnNumber77C M
.77M N
Number77N T
==77U W
request77X _
.77_ `
Number77` f
)77f g
{88 
throw99 
	Validator99 '
.99' (
ErrorValidation99( 7
(997 8
(998 9
$str999 A
,99A B
$str99C h
)99h i
)99i j
;99j k
}:: 
};; 
}<< 
yarnNumberDocument>> 
.>> 
SetCode>> &
(>>& '
request>>' .
.>>. /
Code>>/ 3
)>>3 4
;>>4 5
yarnNumberDocument?? 
.?? 
	SetNumber?? (
(??( )
request??) 0
.??0 1
Number??1 7
)??7 8
;??8 9
yarnNumberDocument@@ 
.@@ 
SetRingType@@ *
(@@* +
request@@+ 2
.@@2 3
RingType@@3 ;
)@@; <
;@@< =
yarnNumberDocumentAA 
.AA 
SetDescriptionAA -
(AA- .
requestAA. 5
.AA5 6
DescriptionAA6 A
)AAA B
;AAB C
awaitCC !
_YarnNumberRepositoryCC '
.CC' (
UpdateCC( .
(CC. /
yarnNumberDocumentCC/ A
)CCA B
;CCB C
_storageEE 
.EE 
SaveEE 
(EE 
)EE 
;EE 
returnGG 
yarnNumberDocumentGG %
;GG% &
}HH 	
}II 
}JJ ú
ÉD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Yarns\CommandHandlers\CreateNewYarnCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
Yarns# (
.( )
CommandHandlers) 8
{ 
public 

class '
CreateNewYarnCommandHandler ,
:- .
ICommandHandler/ >
<> ? 
CreateNewYarnCommand? S
,S T
YarnDocumentU a
>a b
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly #
IYarnDocumentRepository 0#
_yarnDocumentRepository1 H
;H I
public '
CreateNewYarnCommandHandler *
(* +
IStorage+ 3
storage4 ;
); <
{ 	
_storage 
= 
storage 
; #
_yarnDocumentRepository #
=$ %
_storage& .
.. /
GetRepository/ <
<< =#
IYarnDocumentRepository= T
>T U
(U V
)V W
;W X
} 	
public 
async 
Task 
< 
YarnDocument &
>& '
Handle( .
(. / 
CreateNewYarnCommand/ C
requestD K
,K L
CancellationTokenM ^
cancellationToken_ p
)p q
{ 	
var 
exsistingCode 
= #
_yarnDocumentRepository  7
.7 8
Find8 <
(< =
yarn= A
=>B D
yarnE I
.I J
CodeJ N
.N O
EqualsO U
(U V
requestV ]
.] ^
Code^ b
)b c
&&d f
yarnE I
.I J
DeletedJ Q
.Q R
EqualsR X
(X Y
falseY ^
)^ _
)_ `
.` a
Counta f
(f g
)g h
>=i k
$numl m
;m n
if   
(   
exsistingCode   
)   
{!! 
throw"" 
	Validator"" 
.""  
ErrorValidation""  /
(""/ 0
(""0 1
$str""1 7
,""7 8
$str""9 E
+""F G
request""H O
.""O P
Code""P T
+""U V
$str""W g
)""g h
)""h i
;""i j
}## 
var%% 
newYarn%% 
=%% 
new%% 
YarnDocument%% *
(%%* +
Guid%%+ /
.%%/ 0
NewGuid%%0 7
(%%7 8
)%%8 9
,%%9 :
request&&+ 2
.&&2 3
Code&&3 7
,&&7 8
request''+ 2
.''2 3
Name''3 7
,''7 8
request((+ 2
.((2 3
Tags((3 7
,((7 8
new))+ .
MaterialTypeId))/ =
())= >
Guid))> B
.))B C
Parse))C H
())H I
request))I P
.))P Q
MaterialTypeId))Q _
)))_ `
)))` a
,))a b
new**+ .
YarnNumberId**/ ;
(**; <
Guid**< @
.**@ A
Parse**A F
(**F G
request**G N
.**N O
YarnNumberId**O [
)**[ \
)**\ ]
)**] ^
;**^ _
await,, #
_yarnDocumentRepository,, )
.,,) *
Update,,* 0
(,,0 1
newYarn,,1 8
),,8 9
;,,9 :
_storage-- 
.-- 
Save-- 
(-- 
)-- 
;-- 
return// 
newYarn// 
;// 
}00 	
}11 
}22 Œ
âD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Yarns\CommandHandlers\RemoveExsistingYarnCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
Yarns# (
.( )
CommandHandlers) 8
{ 
public 

class -
!RemoveExsistingYarnCommandHandler 2
:3 4
ICommandHandler5 D
<D E&
RemoveExsistingYarnCommandE _
,_ `
YarnDocumenta m
>m n
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly #
IYarnDocumentRepository 0#
_yarnDocumentRepository1 H
;H I
public -
!RemoveExsistingYarnCommandHandler 0
(0 1
IStorage1 9
storage: A
)A B
{ 	
_storage 
= 
storage 
; #
_yarnDocumentRepository #
=$ %
_storage& .
.. /
GetRepository/ <
<< =#
IYarnDocumentRepository= T
>T U
(U V
)V W
;W X
} 	
public 
async 
Task 
< 
YarnDocument &
>& '
Handle( .
(. /&
RemoveExsistingYarnCommand/ I
requestJ Q
,Q R
CancellationTokenS d
cancellationTokene v
)v w
{ 	
var 
exsistingYarn 
= #
_yarnDocumentRepository  7
.7 8
Find8 <
(< =
entity= C
=>D F
entityG M
.M N
IdentityN V
==W Y
requestZ a
.a b
Idb d
)d e
.e f
FirstOrDefaultf t
(t u
)u v
;v w
if 
( 
exsistingYarn 
==  
null! %
)% &
{ 
throw 
	Validator 
.  
ErrorValidation  /
(/ 0
(0 1
$str1 5
,5 6
$str7 J
+K L
requestM T
.T U
IdU W
)W X
)X Y
;Y Z
} 
exsistingYarn!! 
.!! 
Remove!!  
(!!  !
)!!! "
;!!" #
await## #
_yarnDocumentRepository## )
.##) *
Update##* 0
(##0 1
exsistingYarn##1 >
)##> ?
;##? @
_storage%% 
.%% 
Save%% 
(%% 
)%% 
;%% 
return'' 
exsistingYarn''  
;''  !
}(( 	
})) 
}** Ÿ
àD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Application\Yarns\CommandHandlers\UpdateExistingYarnCommandHandler.cs
	namespace 	
Manufactures
 
. 
Application "
." #
Yarns# (
.( )
CommandHandlers) 8
{ 
public 

class ,
 UpdateExistingYarnCommandHandler 1
:2 3
ICommandHandler4 C
<C D&
UpdateExsistingYarnCommandD ^
,^ _
YarnDocument` l
>l m
{ 
private 
readonly 
IStorage !
_storage" *
;* +
private 
readonly #
IYarnDocumentRepository 0#
_yarnDocumentRepository1 H
;H I
public ,
 UpdateExistingYarnCommandHandler /
(/ 0
IStorage0 8
storage9 @
)@ A
{ 	
_storage 
= 
storage 
; #
_yarnDocumentRepository #
=$ %
_storage& .
.. /
GetRepository/ <
<< =#
IYarnDocumentRepository= T
>T U
(U V
)V W
;W X
} 	
public 
async 
Task 
< 
YarnDocument &
>& '
Handle( .
(. /&
UpdateExsistingYarnCommand/ I
requestJ Q
,Q R
CancellationTokenS d
cancellationTokene v
)v w
{ 	
var 
yarnDocument 
= #
_yarnDocumentRepository 6
.6 7
Find7 ;
(; <
yarn< @
=>A C
yarnD H
.H I
IdentityI Q
==R T
requestU \
.\ ]
Id] _
)_ `
.` a
FirstOrDefaulta o
(o p
)p q
;q r
if 
( 
yarnDocument 
== 
null  $
)$ %
{ 
throw   
	Validator   
.    
ErrorValidation    /
(  / 0
(  0 1
$str  1 5
,  5 6
$str  7 J
+  K L
request  M T
.  T U
Id  U W
)  W X
)  X Y
;  Y Z
}!! 
yarnDocument## 
.## 
SetCode##  
(##  !
request##! (
.##( )
Code##) -
)##- .
;##. /
yarnDocument$$ 
.$$ 
SetName$$  
($$  !
request$$! (
.$$( )
Name$$) -
)$$- .
;$$. /
yarnDocument%% 
.%% 
SetTags%%  
(%%  !
request%%! (
.%%( )
Tags%%) -
)%%- .
;%%. /
yarnDocument&& 
.&& 
SetMaterialTypeId&& *
(&&* +
new&&+ .
MaterialTypeId&&/ =
(&&= >
Guid&&> B
.&&B C
Parse&&C H
(&&H I
request&&I P
.&&P Q
MaterialTypeId&&Q _
)&&_ `
)&&` a
)&&a b
;&&b c
yarnDocument'' 
.'' 
SetYarnNumberId'' (
(''( )
new'') ,
YarnNumberId''- 9
(''9 :
Guid'': >
.''> ?
Parse''? D
(''D E
request''E L
.''L M
YarnNumberId''M Y
)''Y Z
)''Z [
)''[ \
;''\ ]
await)) #
_yarnDocumentRepository)) )
.))) *
Update))* 0
())0 1
yarnDocument))1 =
)))= >
;))> ?
_storage** 
.** 
Save** 
(** 
)** 
;** 
return,, 
yarnDocument,, 
;,,  
}-- 	
}.. 
}// 