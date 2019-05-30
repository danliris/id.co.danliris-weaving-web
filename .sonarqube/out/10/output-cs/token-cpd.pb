Í[
dD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Controllers\Api\BeamController.cs
	namespace 	
Manufactures
 
. 
Controllers "
." #
Api# &
{ 
[ 
Produces 
( 
$str  
)  !
]! "
[ 
Route 

(
 
$str 
) 
] 
[ 
ApiController 
] 
[ 
	Authorize 
] 
public 

class 
BeamController 
:  !
ControllerApiBase" 3
{ 
private 
readonly 
IBeamRepository (
_beamRepository) 8
;8 9
public 
BeamController 
( 
IServiceProvider .
serviceProvider/ >
)> ?
: 
base 
( 
serviceProvider "
)" #
{ 	
_beamRepository 
= 
this "
." #
Storage# *
.* +
GetRepository+ 8
<8 9
IBeamRepository9 H
>H I
(I J
)J K
;K L
} 	
[ 	
HttpGet	 
] 
public 
async 
Task 
< 
IActionResult '
>' (
Get) ,
(, -
int- 0
page1 5
=6 7
$num8 9
,9 :
int, /
size0 4
=5 6
$num7 9
,9 :
string  , 2
order  3 8
=  9 :
$str  ; ?
,  ? @
string!!, 2
keyword!!3 :
=!!; <
null!!= A
,!!A B
string"", 2
filter""3 9
="": ;
$str""< @
)""@ A
{## 	
page$$ 
=$$ 
page$$ 
-$$ 
$num$$ 
;$$ 
var%% 
query%% 
=%% 
_beamRepository&& 
.&&  
Query&&  %
.&&% &
OrderByDescending&&& 7
(&&7 8
o&&8 9
=>&&: <
o&&= >
.&&> ?
CreatedDate&&? J
)&&J K
;&&K L
var'' 
beams'' 
='' 
_beamRepository'' '
.''' (
Find''( ,
('', -
query''- 2
)''2 3
.''3 4
Select''4 :
('': ;
o''; <
=>''= ?
new''@ C
BeamDto''D K
(''K L
o''L M
)''M N
)''N O
;''O P
if)) 
()) 
!)) 
string)) 
.)) 
IsNullOrEmpty)) %
())% &
keyword))& -
)))- .
))). /
{** 
beams++ 
=++ 
beams,, 
.,, 
Where,, 
(,,  
entity,,  &
=>,,' )
entity-- 
.-- 
Number-- %
.--% &
Contains--& .
(--. /
keyword--/ 6
,--6 7
StringComparison--8 H
.--H I
OrdinalIgnoreCase--I Z
)--Z [
||--\ ^
entity.. 
... 
Type.. #
...# $
Contains..$ ,
(.., -
keyword..- 4
,..4 5
StringComparison..6 F
...F G
OrdinalIgnoreCase..G X
)..X Y
)..Y Z
;..Z [
}// 
if11 
(11 
!11 
order11 
.11 
Contains11 
(11  
$str11  $
)11$ %
)11% &
{22 

Dictionary33 
<33 
string33 !
,33! "
string33# )
>33) *
orderDictionary33+ :
=33; <
JsonConvert44 
.44  
DeserializeObject44  1
<441 2

Dictionary442 <
<44< =
string44= C
,44C D
string44E K
>44K L
>44L M
(44M N
order44N S
)44S T
;44T U
var55 
key55 
=55 
orderDictionary55 )
.55) *
Keys55* .
.55. /
First55/ 4
(554 5
)555 6
.556 7
	Substring557 @
(55@ A
$num55A B
,55B C
$num55D E
)55E F
.55F G
ToUpper55G N
(55N O
)55O P
+55Q R
orderDictionary66 )
.66) *
Keys66* .
.66. /
First66/ 4
(664 5
)665 6
.666 7
	Substring667 @
(66@ A
$num66A B
)66B C
;66C D
System77 
.77 

Reflection77 !
.77! "
PropertyInfo77" .
prop77/ 3
=774 5
typeof776 <
(77< =
BeamDto77= D
)77D E
.77E F
GetProperty77F Q
(77Q R
key77R U
)77U V
;77V W
if99 
(99 
orderDictionary99 #
.99# $
Values99$ *
.99* +
Contains99+ 3
(993 4
$str994 9
)999 :
)99: ;
{:: 
beams;; 
=;; 
beams;; !
.;;! "
OrderBy;;" )
(;;) *
x;;* +
=>;;, .
prop;;/ 3
.;;3 4
GetValue;;4 <
(;;< =
x;;= >
,;;> ?
null;;@ D
);;D E
);;E F
;;;F G
}<< 
else== 
{>> 
beams?? 
=?? 
beams?? !
.??! "
OrderByDescending??" 3
(??3 4
x??4 5
=>??6 8
prop??9 =
.??= >
GetValue??> F
(??F G
x??G H
,??H I
null??J N
)??N O
)??O P
;??P Q
}@@ 
}AA 
beamsCC 
=CC 
beamsCC 
.CC 
SkipCC 
(CC 
pageCC #
*CC$ %
sizeCC& *
)CC* +
.CC+ ,
TakeCC, 0
(CC0 1
sizeCC1 5
)CC5 6
;CC6 7
intDD 
	totalRowsDD 
=DD 
beamsDD !
.DD! "
CountDD" '
(DD' (
)DD( )
;DD) *
pageEE 
=EE 
pageEE 
+EE 
$numEE 
;EE 
awaitGG 
TaskGG 
.GG 
YieldGG 
(GG 
)GG 
;GG 
returnII 
OkII 
(II 
beamsII 
,II 
infoII !
:II! "
newII# &
{JJ 
pageKK 
,KK 
sizeLL 
,LL 
totalMM 
=MM 
	totalRowsMM !
}NN 
)NN 
;NN 
}OO 	
[QQ 	
HttpGetQQ	 
(QQ 
$strQQ 
)QQ 
]QQ 
publicRR 
asyncRR 
TaskRR 
<RR 
IActionResultRR '
>RR' (
GetRR) ,
(RR, -
stringRR- 3
IdRR4 6
)RR6 7
{SS 	
varTT 
IdentityTT 
=TT 
GuidTT 
.TT  
ParseTT  %
(TT% &
IdTT& (
)TT( )
;TT) *
varUU 
beamUU 
=UU 
_beamRepositoryVV 
.WW 
FindWW 
(WW 
itemWW 
=>WW !
itemWW" &
.WW& '
IdentityWW' /
==WW0 2
IdentityWW3 ;
)WW; <
.XX 
SelectXX 
(XX 
itemXX  
=>XX! #
newXX$ '
BeamDtoXX( /
(XX/ 0
itemXX0 4
)XX4 5
)XX5 6
.YY 
FirstOrDefaultYY #
(YY# $
)YY$ %
;YY% &
await[[ 
Task[[ 
.[[ 
Yield[[ 
([[ 
)[[ 
;[[ 
if]] 
(]] 
beam]] 
==]] 
null]] 
)]] 
{^^ 
return__ 
NotFound__ 
(__  
)__  !
;__! "
}`` 
elseaa 
{bb 
returncc 
Okcc 
(cc 
beamcc 
)cc 
;cc  
}dd 
}ee 	
[gg 	
HttpPostgg	 
]gg 
publichh 
asynchh 
Taskhh 
<hh 
IActionResulthh '
>hh' (
Posthh) -
(hh- .
[hh. /
FromBodyhh/ 7
]hh7 8
AddBeamCommandhh8 F
commandhhG N
)hhN O
{ii 	
varjj 
beamjj 
=jj 
awaitjj 
Mediatorjj %
.jj% &
Sendjj& *
(jj* +
commandjj+ 2
)jj2 3
;jj3 4
returnll 
Okll 
(ll 
beamll 
.ll 
Identityll #
)ll# $
;ll$ %
}mm 	
[oo 	
HttpPutoo	 
(oo 
$stroo 
)oo 
]oo 
publicpp 
asyncpp 
Taskpp 
<pp 
IActionResultpp '
>pp' (
Putpp) ,
(pp, -
stringpp- 3
Idpp4 6
,pp6 7
[qq, -
FromBodyqq- 5
]qq5 6
UpdateBeamCommandqq6 G
commandqqH O
)qqO P
{rr 	
ifss 
(ss 
!ss 
Guidss 
.ss 
TryParsess 
(ss 
Idss !
,ss! "
outss# &
Guidss' +
Identityss, 4
)ss4 5
)ss5 6
{tt 
returnuu 
NotFounduu 
(uu  
)uu  !
;uu! "
}vv 
commandxx 
.xx 
SetIdxx 
(xx 
Identityxx "
)xx" #
;xx# $
varyy 
beamyy 
=yy 
awaityy 
Mediatoryy %
.yy% &
Sendyy& *
(yy* +
commandyy+ 2
)yy2 3
;yy3 4
return{{ 
Ok{{ 
({{ 
beam{{ 
.{{ 
Identity{{ #
){{# $
;{{$ %
}|| 	
[~~ 	

HttpDelete~~	 
(~~ 
$str~~ 
)~~ 
]~~ 
public 
async 
Task 
< 
IActionResult '
>' (
Delete) /
(/ 0
string0 6
Id7 9
)9 :
{
ÄÄ 	
if
ÅÅ 
(
ÅÅ 
!
ÅÅ 
Guid
ÅÅ 
.
ÅÅ 
TryParse
ÅÅ 
(
ÅÅ 
Id
ÅÅ !
,
ÅÅ! "
out
ÅÅ# &
Guid
ÅÅ' +
Identity
ÅÅ, 4
)
ÅÅ4 5
)
ÅÅ5 6
{
ÇÇ 
return
ÉÉ 
NotFound
ÉÉ 
(
ÉÉ  
)
ÉÉ  !
;
ÉÉ! "
}
ÑÑ 
var
ÜÜ 
command
ÜÜ 
=
ÜÜ 
new
ÜÜ 
RemoveBeamCommand
ÜÜ /
(
ÜÜ/ 0
)
ÜÜ0 1
;
ÜÜ1 2
command
áá 
.
áá 
SetId
áá 
(
áá 
Identity
áá "
)
áá" #
;
áá# $
var
ââ 
beam
ââ 
=
ââ 
await
ââ 
Mediator
ââ %
.
ââ% &
Send
ââ& *
(
ââ* +
command
ââ+ 2
)
ââ2 3
;
ââ3 4
return
ãã 
Ok
ãã 
(
ãã 
beam
ãã 
.
ãã 
Identity
ãã #
)
ãã# $
;
ãã$ %
}
åå 	
}
çç 
}éé ‹z
rD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Controllers\Api\DailyOperationLoomController.cs
	namespace 	
Manufactures
 
. 
Controllers "
." #
Api# &
{ 
[ 
Produces 
( 
$str  
)  !
]! "
[ 
Route 

(
 
$str *
)* +
]+ ,
[ 
ApiController 
] 
[ 
	Authorize 
] 
public 

class (
DailyOperationLoomController -
:. /
ControllerApiBase0 A
{ 
private 
readonly )
IDailyOperationLoomRepository 6/
#_dailyOperationalDocumentRepository /
;/ 0
private 
readonly +
IWeavingOrderDocumentRepository 8+
_weavingOrderDocumentRepository +
;+ ,
private   
readonly   )
IFabricConstructionRepository   6+
_constructionDocumentRepository!! +
;!!+ ,
private"" 
readonly"" 
IMachineRepository"" +
_machineRepository## 
;## 
public%% (
DailyOperationLoomController%% +
(%%+ ,
IServiceProvider%%, <
serviceProvider%%= L
,%%L M
IWorkContext&&1 =
workContext&&> I
)&&I J
:'' 
base'' 
('' 
serviceProvider'' "
)''" #
{(( 	/
#_dailyOperationalDocumentRepository)) /
=))0 1
this** 
.** 
Storage** 
.** 
GetRepository** *
<*** +)
IDailyOperationLoomRepository**+ H
>**H I
(**I J
)**J K
;**K L+
_weavingOrderDocumentRepository++ +
=++, -
this,, 
.,, 
Storage,, 
.,, 
GetRepository,, *
<,,* ++
IWeavingOrderDocumentRepository,,+ J
>,,J K
(,,K L
),,L M
;,,M N+
_constructionDocumentRepository-- +
=--, -
this.. 
... 
Storage.. 
... 
GetRepository.. *
<..* +)
IFabricConstructionRepository..+ H
>..H I
(..I J
)..J K
;..K L
_machineRepository// 
=//  
this00 
.00 
Storage00 
.00 
GetRepository00 *
<00* +
IMachineRepository00+ =
>00= >
(00> ?
)00? @
;00@ A
}11 	
[33 	
HttpGet33	 
]33 
public44 
async44 
Task44 
<44 
IActionResult44 '
>44' (
Get44) ,
(44, -
int44- 0
page441 5
=446 7
$num448 9
,449 :
int55- 0
size551 5
=556 7
$num558 :
,55: ;
string66- 3
order664 9
=66: ;
$str66< @
,66@ A
string77- 3
keyword774 ;
=77< =
null77> B
,77B C
string88- 3
filter884 :
=88; <
$str88= A
)88A B
{99 	
page:: 
=:: 
page:: 
-:: 
$num:: 
;:: 
var;; 
query;; 
=;; /
#_dailyOperationalDocumentRepository<< 3
.== 
Query== 
.>> 
Include>> 
(>> 
d>> 
=>>> !
d>>" #
.>># $%
DailyOperationLoomDetails>>$ =
)>>= >
.?? 
Where?? 
(?? 
o?? 
=>?? 
o??  !
.??! " 
DailyOperationStatus??" 6
!=??7 9
	Constants??: C
.??C D
FINISH??D J
)??J K
.@@ 
OrderByDescending@@ &
(@@& '
item@@' +
=>@@, .
item@@/ 3
.@@3 4
CreatedDate@@4 ?
)@@? @
;@@@ A
varAA ,
 dailyOperationalMachineDocumentsAA 0
=AA1 2/
#_dailyOperationalDocumentRepositoryBB 3
.CC 
FindCC 
(CC 
queryCC 
)CC  
;CC  !
varEE 
	resultDtoEE 
=EE 
newEE 
ListEE  $
<EE$ %%
DailyOperationLoomListDtoEE% >
>EE> ?
(EE? @
)EE@ A
;EEA B
foreachGG 
(GG 
varGG 
dailyOperationGG '
inGG( *,
 dailyOperationalMachineDocumentsGG+ K
)GGK L
{HH 
varII 
dateOperatedII  
=II! "
newII# &
DateTimeOffsetII' 5
(II5 6
)II6 7
;II7 8
varKK 
machineDocumentKK #
=KK$ %
awaitLL 
_machineRepositoryLL ,
.MM 
QueryMM 
.NN 
WhereNN 
(NN 
dNN  
=>NN! #
dNN$ %
.NN% &
IdentityNN& .
.NN. /
EqualsNN/ 5
(NN5 6
dailyOperationNN6 D
.NND E
	MachineIdNNE N
.NNN O
ValueNNO T
)NNT U
)NNU V
.OO 
FirstOrDefaultAsyncOO ,
(OO, -
)OO- .
;OO. /
varPP 
orderDocumentPP !
=PP" #
awaitQQ +
_weavingOrderDocumentRepositoryQQ <
.RR 
QueryRR !
.SS 
WhereSS !
(SS! "
oSS" #
=>SS$ &
oSS' (
.SS( )
IdentitySS) 1
.SS1 2
EqualsSS2 8
(SS8 9
dailyOperationSS9 G
.SSG H
OrderIdSSH O
)SSO P
)SSP Q
.TT 
FirstOrDefaultAsyncTT /
(TT/ 0
)TT0 1
;TT1 2
foreachVV 
(VV 
varVV 
detailVV #
inVV$ &
dailyOperationVV' 5
.VV5 6(
DailyOperationMachineDetailsVV6 R
)VVR S
{WW 
ifXX 
(XX 
detailXX 
.XX %
DailyOperationLoomHistoryXX 8
.YY 
DeserializeYY (
<YY( )%
DailyOperationLoomHistoryYY) B
>YYB C
(YYC D
)YYD E
.ZZ  !
MachineStatusZZ! .
==ZZ/ 1'
DailyOperationMachineStatusZZ2 M
.ZZM N
ONENTRYZZN U
)ZZU V
{[[ 
dateOperated\\ $
=\\% &
detail\\' -
.\\- .%
DailyOperationLoomHistory\\. G
.]] 
Deserialize]] (
<]]( )%
DailyOperationLoomHistory]]) B
>]]B C
(]]C D
)]]D E
.]]E F
MachineDate]]F Q
.^^  !
AddHours^^! )
(^^) *
detail^^* 0
.^^0 1%
DailyOperationLoomHistory^^1 J
.__$ %
Deserialize__% 0
<__0 1%
DailyOperationLoomHistory__1 J
>__J K
(__K L
)__L M
.``( )
MachineTime``) 4
.``4 5

TotalHours``5 ?
)``? @
;``@ A
}aa 
}bb 
vardd 
dtodd 
=dd 
newee %
DailyOperationLoomListDtoee 1
(ee1 2
dailyOperationee2 @
,ee@ A
orderDocumentff2 ?
.ff? @
OrderNumberff@ K
,ffK L
machineDocumentgg2 A
.ggA B
MachineNumberggB O
,ggO P
dateOperatedhh2 >
)hh> ?
;hh? @
	resultDtojj 
.jj 
Addjj 
(jj 
dtojj !
)jj! "
;jj" #
}kk 
ifmm 
(mm 
!mm 
stringmm 
.mm 
IsNullOrEmptymm %
(mm% &
keywordmm& -
)mm- .
)mm. /
{nn 
	resultDtooo 
=oo 
	resultDtopp 
.pp 
Wherepp #
(pp# $
entitypp$ *
=>pp+ -
entitypp. 4
.qq0 1
OrderNumberqq1 <
.rr0 1
Containsrr1 9
(rr9 :
keywordrr: A
,rrA B
StringComparisonss: J
.tt< =
OrdinalIgnoreCasett= N
)ttN O
||ttP R
entityuu. 4
.vv0 1
MachineNumbervv1 >
.ww0 1
Containsww1 9
(ww9 :
keywordww: A
,wwA B
StringComparisonxx: J
.yy< =
OrdinalIgnoreCaseyy= N
)yyN O
)yyO P
.zz 
ToListzz $
(zz$ %
)zz% &
;zz& '
}{{ 
if}} 
(}} 
!}} 
order}} 
.}} 
Contains}} 
(}}  
$str}}  $
)}}$ %
)}}% &
{~~ 

Dictionary 
< 
string !
,! "
string# )
>) *
orderDictionary+ :
=; <
JsonConvert
ÄÄ 
.
ÄÄ  
DeserializeObject
ÄÄ  1
<
ÄÄ1 2

Dictionary
ÄÄ2 <
<
ÄÄ< =
string
ÄÄ= C
,
ÄÄC D
string
ÄÄE K
>
ÄÄK L
>
ÄÄL M
(
ÄÄM N
order
ÄÄN S
)
ÄÄS T
;
ÄÄT U
var
ÅÅ 
key
ÅÅ 
=
ÅÅ 
orderDictionary
ÇÇ #
.
ÇÇ# $
Keys
ÇÇ$ (
.
ÇÇ( )
First
ÇÇ) .
(
ÇÇ. /
)
ÇÇ/ 0
.
ÇÇ0 1
	Substring
ÇÇ1 :
(
ÇÇ: ;
$num
ÇÇ; <
,
ÇÇ< =
$num
ÇÇ> ?
)
ÇÇ? @
.
ÇÇ@ A
ToUpper
ÇÇA H
(
ÇÇH I
)
ÇÇI J
+
ÇÇK L
orderDictionary
ÉÉ #
.
ÉÉ# $
Keys
ÉÉ$ (
.
ÉÉ( )
First
ÉÉ) .
(
ÉÉ. /
)
ÉÉ/ 0
.
ÉÉ0 1
	Substring
ÉÉ1 :
(
ÉÉ: ;
$num
ÉÉ; <
)
ÉÉ< =
;
ÉÉ= >
System
ÑÑ 
.
ÑÑ 

Reflection
ÑÑ !
.
ÑÑ! "
PropertyInfo
ÑÑ" .
prop
ÑÑ/ 3
=
ÑÑ4 5
typeof
ÖÖ 
(
ÖÖ '
DailyOperationLoomListDto
ÖÖ 4
)
ÖÖ4 5
.
ÖÖ5 6
GetProperty
ÖÖ6 A
(
ÖÖA B
key
ÖÖB E
)
ÖÖE F
;
ÖÖF G
if
áá 
(
áá 
orderDictionary
áá #
.
áá# $
Values
áá$ *
.
áá* +
Contains
áá+ 3
(
áá3 4
$str
áá4 9
)
áá9 :
)
áá: ;
{
àà 
	resultDto
ââ 
=
ââ 
	resultDto
ää !
.
ãã 
OrderBy
ãã $
(
ãã$ %
x
ãã% &
=>
ãã' )
prop
ãã* .
.
ãã. /
GetValue
ãã/ 7
(
ãã7 8
x
ãã8 9
,
ãã9 :
null
ãã; ?
)
ãã? @
)
ãã@ A
.
åå 
ToList
åå #
(
åå# $
)
åå$ %
;
åå% &
}
çç 
else
éé 
{
èè 
	resultDto
êê 
=
êê 
	resultDto
ëë !
.
íí 
OrderByDescending
íí .
(
íí. /
x
íí/ 0
=>
íí1 3
prop
íí4 8
.
íí8 9
GetValue
íí9 A
(
ííA B
x
ííB C
,
ííC D
null
ííE I
)
ííI J
)
ííJ K
.
ìì 
ToList
ìì #
(
ìì# $
)
ìì$ %
;
ìì% &
}
îî 
}
ïï 
	resultDto
óó 
=
óó 
	resultDto
òò 
.
òò 
Skip
òò 
(
òò 
page
òò #
*
òò$ %
size
òò& *
)
òò* +
.
òò+ ,
Take
òò, 0
(
òò0 1
size
òò1 5
)
òò5 6
.
òò6 7
ToList
òò7 =
(
òò= >
)
òò> ?
;
òò? @
int
ôô 
	totalRows
ôô 
=
ôô 
	resultDto
ôô %
.
ôô% &
Count
ôô& +
(
ôô+ ,
)
ôô, -
;
ôô- .
page
öö 
=
öö 
page
öö 
+
öö 
$num
öö 
;
öö 
await
úú 
Task
úú 
.
úú 
Yield
úú 
(
úú 
)
úú 
;
úú 
return
ûû 
Ok
ûû 
(
ûû 
	resultDto
ûû 
,
ûû  
info
ûû! %
:
ûû% &
new
ûû' *
{
üü 
page
†† 
,
†† 
size
°° 
,
°° 
total
¢¢ 
=
¢¢ 
	totalRows
¢¢ !
}
££ 
)
££ 
;
££ 
}
§§ 	
[
¶¶ 	
HttpGet
¶¶	 
(
¶¶ 
$str
¶¶ 
)
¶¶ 
]
¶¶ 
public
ßß 
async
ßß 
Task
ßß 
<
ßß 
IActionResult
ßß '
>
ßß' (
Get
ßß) ,
(
ßß, -
string
ßß- 3
Id
ßß4 6
)
ßß6 7
{
®® 	
var
©© 
Identity
©© 
=
©© 
Guid
©© 
.
©©  
Parse
©©  %
(
©©% &
Id
©©& (
)
©©( )
;
©©) *
var
™™ 
query
™™ 
=
™™ 1
#_dailyOperationalDocumentRepository
´´ 3
.
¨¨ 
Query
¨¨ 
.
≠≠ 
Include
≠≠ 
(
≠≠ 
p
≠≠ 
=>
≠≠ !
p
≠≠" #
.
≠≠# $'
DailyOperationLoomDetails
≠≠$ =
)
≠≠= >
.
ÆÆ 
Where
ÆÆ 
(
ÆÆ 
o
ÆÆ 
=>
ÆÆ 
o
ÆÆ  !
.
ÆÆ! "
Identity
ÆÆ" *
==
ÆÆ+ -
Identity
ÆÆ. 6
)
ÆÆ6 7
;
ÆÆ7 8
var
ØØ "
dailyOperationalLoom
ØØ $
=
ØØ% &1
#_dailyOperationalDocumentRepository
∞∞ 3
.
±± 
Find
±± 
(
±± 
query
±± 
)
±±  
.
≤≤ 
FirstOrDefault
≤≤ #
(
≤≤# $
)
≤≤$ %
;
≤≤% &
await
¥¥ 
Task
¥¥ 
.
¥¥ 
Yield
¥¥ 
(
¥¥ 
)
¥¥ 
;
¥¥ 
if
∂∂ 
(
∂∂ 
Identity
∂∂ 
==
∂∂ 
null
∂∂  
||
∂∂! #"
dailyOperationalLoom
∂∂$ 8
==
∂∂9 ;
null
∂∂< @
)
∂∂@ A
{
∑∑ 
return
∏∏ 
NotFound
∏∏ 
(
∏∏  
)
∏∏  !
;
∏∏! "
}
ππ 
else
∫∫ 
{
ªª 
return
ºº 
Ok
ºº 
(
ºº "
dailyOperationalLoom
ºº .
)
ºº. /
;
ºº/ 0
}
ΩΩ 
}
ææ 	
[
¿¿ 	
HttpPost
¿¿	 
(
¿¿ 
$str
¿¿ !
)
¿¿! "
]
¿¿" #
public
¡¡ 
async
¡¡ 
Task
¡¡ 
<
¡¡ 
IActionResult
¡¡ '
>
¡¡' (
Post
¡¡) -
(
¡¡- .
[
¡¡. /
FromBody
¡¡/ 7
]
¡¡7 8-
AddNewDailyOperationLoomCommand
¡¡8 W
command
¡¡X _
)
¡¡_ `
{
¬¬ 	
var
√√ 0
"newDailyOperationalMachineDocument
√√ 2
=
√√3 4
await
√√5 :
Mediator
√√; C
.
√√C D
Send
√√D H
(
√√H I
command
√√I P
)
√√P Q
;
√√Q R
return
≈≈ 
Ok
≈≈ 
(
≈≈ 0
"newDailyOperationalMachineDocument
≈≈ 8
.
≈≈8 9
Identity
≈≈9 A
)
≈≈A B
;
≈≈B C
}
∆∆ 	
}
«« 
}»» ≤á
tD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Controllers\Api\DailyOperationSizingController.cs
	namespace 	
Manufactures
 
. 
Controllers "
." #
Api# &
{ 
[ 
Produces 
( 
$str  
)  !
]! "
[ 
Route 

(
 
$str ,
), -
]- .
[ 
ApiController 
] 
[ 
	Authorize 
] 
public 

class *
DailyOperationSizingController /
:0 1
ControllerApiBase2 C
{ 
private 
readonly +
IDailyOperationSizingRepository 83
'_dailyOperationSizingDocumentRepository 3
;3 4
private 
readonly 
IMachineRepository +
_machineRepository 
; 
private 
readonly )
IFabricConstructionRepository 6+
_constructionDocumentRepository +
;+ ,
private   
readonly   
IShiftRepository   )$
_shiftDocumentRepository!! $
;!!$ %
public## *
DailyOperationSizingController## -
(##- .
IServiceProvider##. >
serviceProvider##? N
,##N O
IWorkContext$$1 =
workContext$$> I
)$$I J
:%% 
base%% 
(%% 
serviceProvider%% "
)%%" #
{&& 	3
'_dailyOperationSizingDocumentRepository'' 3
=''4 5
this(( 
.(( 
Storage(( 
.(( 
GetRepository(( *
<((* ++
IDailyOperationSizingRepository((+ J
>((J K
(((K L
)((L M
;((M N
_machineRepository)) 
=))  
this** 
.** 
Storage** 
.** 
GetRepository** *
<*** +
IMachineRepository**+ =
>**= >
(**> ?
)**? @
;**@ A+
_constructionDocumentRepository++ +
=++, -
this,, 
.,, 
Storage,, 
.,, 
GetRepository,, *
<,,* +)
IFabricConstructionRepository,,+ H
>,,H I
(,,I J
),,J K
;,,K L$
_shiftDocumentRepository-- $
=--% &
this.. 
... 
Storage.. 
... 
GetRepository.. *
<..* +
IShiftRepository..+ ;
>..; <
(..< =
)..= >
;..> ?
}// 	
[11 	
HttpGet11	 
]11 
public22 
async22 
Task22 
<22 
IActionResult22 '
>22' (
Get22) ,
(22, -
int22- 0
page221 5
=226 7
$num228 9
,229 :
int33- 0
size331 5
=336 7
$num338 :
,33: ;
string44- 3
order444 9
=44: ;
$str44< @
,44@ A
string55- 3
keyword554 ;
=55< =
null55> B
,55B C
string66- 3
filter664 :
=66; <
$str66= A
)66A B
{77 	
page88 
=88 
page88 
-88 
$num88 
;88 
var99 
domQuery99 
=99 3
'_dailyOperationSizingDocumentRepository:: 7
.;; 
Query;; 
.<< 
OrderByDescending<< &
(<<& '
item<<' +
=><<, .
item<</ 3
.<<3 4
CreatedDate<<4 ?
)<<? @
;<<@ A
var== )
dailyOperationSizingDocuments== -
===. /3
'_dailyOperationSizingDocumentRepository>> 7
.?? 
Find?? 
(?? 
domQuery?? "
.??" #
Include??# *
(??* +
d??+ ,
=>??- /
d??0 1
.??1 2'
DailyOperationSizingDetails??2 M
)??M N
)??N O
;??O P
varAA 
	resultDtoAA 
=AA 
newAA 
ListAA  $
<AA$ %'
DailyOperationSizingListDtoAA% @
>AA@ A
(AAA B
)AAB C
;AAC D
foreachCC 
(CC 
varCC 
dailyOperationCC '
inCC( *)
dailyOperationSizingDocumentsCC+ H
)CCH I
{DD 
foreachFF 
(FF 
varFF 
detailFF #
inFF$ &
dailyOperationFF' 5
.FF5 6'
DailyOperationSizingDetailsFF6 Q
)FFQ R
{GG 
varHH 
dtoHH 
=HH 
newHH !'
DailyOperationSizingListDtoHH" =
(HH= >
dailyOperationHH> L
,HHL M
detailHHN T
)HHT U
;HHU V
	resultDtoJJ 
.JJ 
AddJJ !
(JJ! "
dtoJJ" %
)JJ% &
;JJ& '
}LL 
}MM 
ifOO 
(OO 
!OO 
orderOO 
.OO 
ContainsOO 
(OO  
$strOO  $
)OO$ %
)OO% &
{PP 

DictionaryQQ 
<QQ 
stringQQ !
,QQ! "
stringQQ# )
>QQ) *
orderDictionaryQQ+ :
=QQ; <
JsonConvertQQ= H
.QQH I
DeserializeObjectQQI Z
<QQZ [

DictionaryQQ[ e
<QQe f
stringQQf l
,QQl m
stringQQn t
>QQt u
>QQu v
(QQv w
orderQQw |
)QQ| }
;QQ} ~
varRR 
keyRR 
=RR 
orderDictionaryRR )
.RR) *
KeysRR* .
.RR. /
FirstRR/ 4
(RR4 5
)RR5 6
.RR6 7
	SubstringRR7 @
(RR@ A
$numRRA B
,RRB C
$numRRD E
)RRE F
.RRF G
ToUpperRRG N
(RRN O
)RRO P
+RRQ R
orderDictionaryRRS b
.RRb c
KeysRRc g
.RRg h
FirstRRh m
(RRm n
)RRn o
.RRo p
	SubstringRRp y
(RRy z
$numRRz {
)RR{ |
;RR| }
SystemSS 
.SS 

ReflectionSS !
.SS! "
PropertyInfoSS" .
propSS/ 3
=SS4 5
typeofTT 
(TT '
DailyOperationSizingListDtoTT 6
)TT6 7
.TT7 8
GetPropertyTT8 C
(TTC D
keyTTD G
)TTG H
;TTH I
ifVV 
(VV 
orderDictionaryVV #
.VV# $
ValuesVV$ *
.VV* +
ContainsVV+ 3
(VV3 4
$strVV4 9
)VV9 :
)VV: ;
{WW 
	resultDtoXX 
=XX 
	resultDtoYY !
.ZZ 
OrderByZZ $
(ZZ$ %
xZZ% &
=>ZZ' )
propZZ* .
.ZZ. /
GetValueZZ/ 7
(ZZ7 8
xZZ8 9
,ZZ9 :
nullZZ; ?
)ZZ? @
)ZZ@ A
.[[ 
ToList[[ #
([[# $
)[[$ %
;[[% &
}\\ 
else]] 
{^^ 
	resultDto__ 
=__ 
	resultDto`` !
.aa 
OrderByDescendingaa .
(aa. /
xaa/ 0
=>aa1 3
propaa4 8
.aa8 9
GetValueaa9 A
(aaA B
xaaB C
,aaC D
nullaaE I
)aaI J
)aaJ K
.bb 
ToListbb #
(bb# $
)bb$ %
;bb% &
}cc 
}dd 
	resultDtoff 
=ff 
	resultDtogg 
.gg 
Skipgg 
(gg 
pagegg #
*gg$ %
sizegg& *
)gg* +
.gg+ ,
Takegg, 0
(gg0 1
sizegg1 5
)gg5 6
.gg6 7
ToListgg7 =
(gg= >
)gg> ?
;gg? @
inthh 
	totalRowshh 
=hh 
	resultDtohh %
.hh% &
Counthh& +
(hh+ ,
)hh, -
;hh- .
pageii 
=ii 
pageii 
+ii 
$numii 
;ii 
awaitkk 
Taskkk 
.kk 
Yieldkk 
(kk 
)kk 
;kk 
returnmm 
Okmm 
(mm 
	resultDtomm 
,mm  
infomm! %
:mm% &
newmm' *
{nn 
pageoo 
,oo 
sizepp 
,pp 
totalqq 
=qq 
	totalRowsqq !
}rr 
)rr 
;rr 
}ss 	
[uu 	
HttpGetuu	 
(uu 
$struu 
)uu 
]uu 
publicvv 
asyncvv 
Taskvv 
<vv 
IActionResultvv '
>vv' (
Getvv) ,
(vv, -
stringvv- 3
Idvv4 6
)vv6 7
{ww 	
varxx 
Identityxx 
=xx 
Guidxx 
.xx  
Parsexx  %
(xx% &
Idxx& (
)xx( )
;xx) *
varyy 
queryyy 
=yy 3
'_dailyOperationSizingDocumentRepositoryyy ?
.yy? @
Queryyy@ E
;yyE F
varzz (
dailyOperationSizingDocumentzz ,
=zz- .3
'_dailyOperationSizingDocumentRepository{{ 7
.|| 
Find|| 
(|| 
query|| 
.||  
Include||  '
(||' (
p||( )
=>||* ,
p||- .
.||. /'
DailyOperationSizingDetails||/ J
)||J K
)||K L
.}} 
Where}} 
(}} 
o}} 
=>}} 
o}}  !
.}}! "
Identity}}" *
==}}+ -
Identity}}. 6
)}}6 7
.~~ 
FirstOrDefault~~ #
(~~# $
)~~$ %
;~~% &
await
ÄÄ 
Task
ÄÄ 
.
ÄÄ 
Yield
ÄÄ 
(
ÄÄ 
)
ÄÄ 
;
ÄÄ 
if
ÇÇ 
(
ÇÇ 
Identity
ÇÇ 
==
ÇÇ 
null
ÇÇ  
)
ÇÇ  !
{
ÉÉ 
return
ÑÑ 
NotFound
ÑÑ 
(
ÑÑ  
)
ÑÑ  !
;
ÑÑ! "
}
ÖÖ 
else
ÜÜ 
{
áá 
return
àà 
Ok
àà 
(
àà *
dailyOperationSizingDocument
àà 6
)
àà6 7
;
àà7 8
}
ââ 
}
ää 	
[
åå 	
HttpPost
åå	 
]
åå 
public
çç 
async
çç 
Task
çç 
<
çç 
IActionResult
çç '
>
çç' (
Post
çç) -
(
çç- .
[
çç. /
FromBody
çç/ 7
]
çç7 81
#NewEntryDailyOperationSizingCommand
çç8 [
command
çç\ c
)
ççc d
{
éé 	
var
èè -
newDailyOperationSizingDocument
èè /
=
èè0 1
await
èè2 7
Mediator
èè8 @
.
èè@ A
Send
èèA E
(
èèE F
command
èèF M
)
èèM N
;
èèN O
return
ëë 
Ok
ëë 
(
ëë -
newDailyOperationSizingDocument
ëë 5
.
ëë5 6
Identity
ëë6 >
)
ëë> ?
;
ëë? @
}
íí 	
[
îî 	
HttpPut
îî	 
(
îî 
$str
îî 
)
îî 
]
îî 
public
ïï 
async
ïï 
Task
ïï 
<
ïï 
IActionResult
ïï '
>
ïï' (
Put
ïï) ,
(
ïï, -
string
ïï- 3
Id
ïï4 6
,
ïï6 7
[
ññ, -
FromBody
ññ- 5
]
ññ5 64
&UpdateStartDailyOperationSizingCommand
ññ6 \
command
ññ] d
)
ññd e
{
óó 	
if
òò 
(
òò 
!
òò 
Guid
òò 
.
òò 
TryParse
òò 
(
òò 
Id
òò !
,
òò! "
out
òò# &
Guid
òò' +

documentId
òò, 6
)
òò6 7
)
òò7 8
{
ôô 
return
öö 
NotFound
öö 
(
öö  
)
öö  !
;
öö! "
}
õõ 
command
úú 
.
úú 
SetId
úú 
(
úú 

documentId
úú $
)
úú$ %
;
úú% &
var
ùù 5
'updateStartDailyOperationSizingDocument
ùù 7
=
ùù8 9
await
ùù: ?
Mediator
ùù@ H
.
ùùH I
Send
ùùI M
(
ùùM N
command
ùùN U
)
ùùU V
;
ùùV W
return
üü 
Ok
üü 
(
üü 5
'updateStartDailyOperationSizingDocument
üü =
.
üü= >
Identity
üü> F
)
üüF G
;
üüG H
}
†† 	
[
¢¢ 	
HttpPut
¢¢	 
(
¢¢ 
$str
¢¢ 
)
¢¢ 
]
¢¢ 
public
££ 
async
££ 
Task
££ 
<
££ 
IActionResult
££ '
>
££' (
Put
££) ,
(
££, -
string
££- 3
Id
££4 6
,
££6 7
[
§§- .
FromBody
§§. 6
]
§§6 74
&UpdatePauseDailyOperationSizingCommand
§§7 ]
command
§§^ e
)
§§e f
{
•• 	
if
¶¶ 
(
¶¶ 
!
¶¶ 
Guid
¶¶ 
.
¶¶ 
TryParse
¶¶ 
(
¶¶ 
Id
¶¶ !
,
¶¶! "
out
¶¶# &
Guid
¶¶' +

documentId
¶¶, 6
)
¶¶6 7
)
¶¶7 8
{
ßß 
return
®® 
NotFound
®® 
(
®®  
)
®®  !
;
®®! "
}
©© 
command
™™ 
.
™™ 
SetId
™™ 
(
™™ 

documentId
™™ $
)
™™$ %
;
™™% &
var
´´ 5
'updatePauseDailyOperationSizingDocument
´´ 7
=
´´8 9
await
´´: ?
Mediator
´´@ H
.
´´H I
Send
´´I M
(
´´M N
command
´´N U
)
´´U V
;
´´V W
return
≠≠ 
Ok
≠≠ 
(
≠≠ 5
'updatePauseDailyOperationSizingDocument
≠≠ =
.
≠≠= >
Identity
≠≠> F
)
≠≠F G
;
≠≠G H
}
ÆÆ 	
[
∞∞ 	
HttpPut
∞∞	 
(
∞∞ 
$str
∞∞ 
)
∞∞ 
]
∞∞  
public
±± 
async
±± 
Task
±± 
<
±± 
IActionResult
±± '
>
±±' (
Put
±±) ,
(
±±, -
string
±±- 3
Id
±±4 6
,
±±6 7
[
≤≤- .
FromBody
≤≤. 6
]
≤≤6 75
'UpdateResumeDailyOperationSizingCommand
≤≤7 ^
command
≤≤_ f
)
≤≤f g
{
≥≥ 	
if
¥¥ 
(
¥¥ 
!
¥¥ 
Guid
¥¥ 
.
¥¥ 
TryParse
¥¥ 
(
¥¥ 
Id
¥¥ !
,
¥¥! "
out
¥¥# &
Guid
¥¥' +

documentId
¥¥, 6
)
¥¥6 7
)
¥¥7 8
{
µµ 
return
∂∂ 
NotFound
∂∂ 
(
∂∂  
)
∂∂  !
;
∂∂! "
}
∑∑ 
command
∏∏ 
.
∏∏ 
SetId
∏∏ 
(
∏∏ 

documentId
∏∏ $
)
∏∏$ %
;
∏∏% &
var
ππ 6
(updateResumeDailyOperationSizingDocument
ππ 8
=
ππ9 :
await
ππ; @
Mediator
ππA I
.
ππI J
Send
ππJ N
(
ππN O
command
ππO V
)
ππV W
;
ππW X
return
ªª 
Ok
ªª 
(
ªª 6
(updateResumeDailyOperationSizingDocument
ªª >
.
ªª> ?
Identity
ªª? G
)
ªªG H
;
ªªH I
}
ºº 	
[
ææ 	
HttpPut
ææ	 
(
ææ 
$str
ææ 
)
ææ 
]
ææ 
public
øø 
async
øø 
Task
øø 
<
øø 
IActionResult
øø '
>
øø' (
Put
øø) ,
(
øø, -
string
øø- 3
Id
øø4 6
,
øø6 7
[
¿¿- .
FromBody
¿¿. 6
]
¿¿6 79
+UpdateDoffFinishDailyOperationSizingCommand
¿¿7 b
command
¿¿c j
)
¿¿j k
{
¡¡ 	
if
¬¬ 
(
¬¬ 
!
¬¬ 
Guid
¬¬ 
.
¬¬ 
TryParse
¬¬ 
(
¬¬ 
Id
¬¬ !
,
¬¬! "
out
¬¬# &
Guid
¬¬' +

documentId
¬¬, 6
)
¬¬6 7
)
¬¬7 8
{
√√ 
return
ƒƒ 
NotFound
ƒƒ 
(
ƒƒ  
)
ƒƒ  !
;
ƒƒ! "
}
≈≈ 
command
∆∆ 
.
∆∆ 
SetId
∆∆ 
(
∆∆ 

documentId
∆∆ $
)
∆∆$ %
;
∆∆% &
var
«« 4
&updateDoffDailyOperationSizingDocument
«« 6
=
««7 8
await
««9 >
Mediator
««? G
.
««G H
Send
««H L
(
««L M
command
««M T
)
««T U
;
««U V
return
…… 
Ok
…… 
(
…… 4
&updateDoffDailyOperationSizingDocument
…… <
.
……< =
Identity
……= E
)
……E F
;
……F G
}
   	
}
ÀÀ 
}ÃÃ îc
jD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Controllers\Api\EstimationController.cs
	namespace 	
Manufactures
 
. 
Controllers "
." #
Api# &
{ 
[ 
Produces 
( 
$str  
)  !
]! "
[ 
Route 

(
 
$str +
)+ ,
], -
[ 
ApiController 
] 
[ 
	Authorize 
] 
public 

class  
EstimationController %
:& '
ControllerApiBase( 9
{ 
private 
readonly (
IEstimationProductRepository 5(
_estimationProductRepository6 R
;R S
public  
EstimationController #
(# $
IServiceProvider$ 4
serviceProvider5 D
,D E
IWorkContext$ 0
workContext1 <
)< =
:> ?
base@ D
(D E
serviceProviderE T
)T U
{ 	(
_estimationProductRepository (
=) *
this 
. 
Storage 
. 
GetRepository *
<* +(
IEstimationProductRepository+ G
>G H
(H I
)I J
;J K
} 	
[!! 	
HttpGet!!	 
]!! 
public"" 
async"" 
Task"" 
<"" 
IActionResult"" '
>""' (
Get"") ,
("", -
int""- 0
page""1 5
=""6 7
$num""8 9
,""9 :
int##- 0
size##1 5
=##6 7
$num##8 :
,##: ;
string$$- 3
order$$4 9
=$$: ;
$str$$< @
,$$@ A
string%%- 3
keyword%%4 ;
=%%< =
null%%> B
,%%B C
string&&- 3
filter&&4 :
=&&; <
$str&&= A
)&&A B
{'' 	
page(( 
=(( 
page(( 
-(( 
$num(( 
;(( 
var)) 
query)) 
=)) (
_estimationProductRepository** ,
.**, -
Query**- 2
.**2 3
OrderByDescending**3 D
(**D E
item**E I
=>**J L
item**M Q
.**Q R
CreatedDate**R ]
)**] ^
;**^ _
var++ 
estimationDocument++ "
=++# $(
_estimationProductRepository,, ,
.,,, -
Find,,- 1
(,,1 2
query,,2 7
.,,7 8
Include,,8 ?
(,,? @
p,,@ A
=>,,B D
p,,E F
.,,F G
EstimationProducts,,G Y
),,Y Z
),,Z [
.--, -
Select--- 3
(--3 4
item--4 8
=>--9 ;
new--< ?
ListEstimationDto--@ Q
(--Q R
item--R V
)--V W
)--W X
;--X Y
if// 
(// 
!// 
string// 
.// 
IsNullOrEmpty// %
(//% &
keyword//& -
)//- .
)//. /
{00 
estimationDocument11 "
=11# $
estimationDocument22 &
.33 
Where33 
(33 
entity33 %
=>33& (
entity33) /
.33/ 0
EstimationNumber330 @
.33@ A
Contains33A I
(33I J
keyword33J Q
,33Q R
StringComparison44J Z
.44Z [
OrdinalIgnoreCase44[ l
)44l m
)44m n
;44n o
}55 
if77 
(77 
!77 
order77 
.77 
Contains77 
(77  
$str77  $
)77$ %
)77% &
{88 

Dictionary99 
<99 
string99 !
,99! "
string99# )
>99) *
orderDictionary99+ :
=99; <
JsonConvert:: 
.::  
DeserializeObject::  1
<::1 2

Dictionary::2 <
<::< =
string::= C
,::C D
string::E K
>::K L
>::L M
(::M N
order::N S
)::S T
;::T U
var;; 
key;; 
=;; 
orderDictionary;; )
.;;) *
Keys;;* .
.;;. /
First;;/ 4
(;;4 5
);;5 6
.;;6 7
	Substring;;7 @
(;;@ A
$num;;A B
,;;B C
$num;;D E
);;E F
.;;F G
ToUpper;;G N
(;;N O
);;O P
+;;Q R
orderDictionary<< )
.<<) *
Keys<<* .
.<<. /
First<</ 4
(<<4 5
)<<5 6
.<<6 7
	Substring<<7 @
(<<@ A
$num<<A B
)<<B C
;<<C D
System== 
.== 

Reflection== !
.==! "
PropertyInfo==" .
prop==/ 3
===4 5
typeof==6 <
(==< =
ListEstimationDto=== N
)==N O
.==O P
GetProperty==P [
(==[ \
key==\ _
)==_ `
;==` a
if?? 
(?? 
orderDictionary?? #
.??# $
Values??$ *
.??* +
Contains??+ 3
(??3 4
$str??4 9
)??9 :
)??: ;
{@@ 
estimationDocumentAA &
=AA' (
estimationDocumentBB *
.BB* +
OrderByBB+ 2
(BB2 3
xBB3 4
=>BB5 7
propBB8 <
.BB< =
GetValueBB= E
(BBE F
xBBF G
,BBG H
nullBBI M
)BBM N
)BBN O
;BBO P
}CC 
elseDD 
{EE 
estimationDocumentFF &
=FF' (
estimationDocumentGG *
.GG* +
OrderByDescendingGG+ <
(GG< =
xGG= >
=>GG? A
propGGB F
.GGF G
GetValueGGG O
(GGO P
xGGP Q
,GGQ R
nullGGS W
)GGW X
)GGX Y
;GGY Z
}HH 
}II 
estimationDocumentKK 
=KK  
estimationDocumentLL "
.LL" #
SkipLL# '
(LL' (
pageLL( ,
*LL- .
sizeLL/ 3
)LL3 4
.LL4 5
TakeLL5 9
(LL9 :
sizeLL: >
)LL> ?
;LL? @
intMM 
	totalRowsMM 
=MM 
estimationDocumentMM .
.MM. /
CountMM/ 4
(MM4 5
)MM5 6
;MM6 7
pageNN 
=NN 
pageNN 
+NN 
$numNN 
;NN 
awaitPP 
TaskPP 
.PP 
YieldPP 
(PP 
)PP 
;PP 
returnRR 
OkRR 
(RR 
estimationDocumentRR (
,RR( )
infoRR* .
:RR. /
newRR0 3
{SS 
pageTT 
,TT 
sizeUU 
,UU 
totalVV 
=VV 
	totalRowsVV !
}WW 
)WW 
;WW 
}XX 	
[ZZ 	
HttpGetZZ	 
(ZZ 
$strZZ 
)ZZ 
]ZZ 
public[[ 
async[[ 
Task[[ 
<[[ 
IActionResult[[ '
>[[' (
Get[[) ,
([[, -
string[[- 3
Id[[4 6
)[[6 7
{\\ 	
var]] 
Identity]] 
=]] 
Guid]] 
.]]  
Parse]]  %
(]]% &
Id]]& (
)]]( )
;]]) *
var^^ 
query^^ 
=^^ (
_estimationProductRepository^^ 4
.^^4 5
Query^^5 :
;^^: ;
var__ 
estimationDocument__ "
=__# $(
_estimationProductRepository`` ,
.``, -
Find``- 1
(``1 2
query``2 7
.``7 8
Include``8 ?
(``? @
p``@ A
=>``B D
p``E F
.``F G
EstimationProducts``G Y
)``Y Z
)``Z [
.aa, -
Whereaa- 2
(aa2 3
oaa3 4
=>aa5 7
oaa8 9
.aa9 :
Identityaa: B
==aaC E
IdentityaaF N
)aaN O
.bb, -
Selectbb- 3
(bb3 4
itembb4 8
=>bb9 ;
newbb< ?
EstimationByIdDtobb@ Q
(bbQ R
itembbR V
)bbV W
)bbW X
.cc, -
FirstOrDefaultcc- ;
(cc; <
)cc< =
;cc= >
awaitdd 
Taskdd 
.dd 
Yielddd 
(dd 
)dd 
;dd 
ifff 
(ff 
Identityff 
==ff 
nullff  
)ff  !
{gg 
returnhh 
NotFoundhh 
(hh  
)hh  !
;hh! "
}ii 
elsejj 
{kk 
returnll 
Okll 
(ll 
estimationDocumentll ,
)ll, -
;ll- .
}mm 
}nn 	
[pp 	
HttpPostpp	 
]pp 
publicqq 
asyncqq 
Taskqq 
<qq 
IActionResultqq '
>qq' (
Postqq) -
(qq- .
[qq. /
FromBodyqq/ 7
]qq7 8#
AddNewEstimationCommandqq8 O
commandqqP W
)qqW X
{rr 	
varss !
newEstimationDocumentss %
=ss& '
awaitss( -
Mediatorss. 6
.ss6 7
Sendss7 ;
(ss; <
commandss< C
)ssC D
;ssD E
returnuu 
Okuu 
(uu !
newEstimationDocumentuu +
.uu+ ,
Identityuu, 4
)uu4 5
;uu5 6
}vv 	
[xx 	
HttpPutxx	 
(xx 
$strxx 
)xx 
]xx 
publicyy 
asyncyy 
Taskyy 
<yy 
IActionResultyy '
>yy' (
Putyy) ,
(yy, -
stringyy- 3
Idyy4 6
,yy6 7
[zz- .
FromBodyzz. 6
]zz6 7*
UpdateEstimationProductCommandzz7 U
commandzzV ]
)zz] ^
{{{ 	
if|| 
(|| 
!|| 
Guid|| 
.|| 
TryParse|| 
(|| 
Id|| !
,||! "
out||# &
Guid||' +

documentId||, 6
)||6 7
)||7 8
{}} 
return~~ 
NotFound~~ 
(~~  
)~~  !
;~~! "
} 
command
ÅÅ 
.
ÅÅ 
SetId
ÅÅ 
(
ÅÅ 

documentId
ÅÅ $
)
ÅÅ$ %
;
ÅÅ% &
var
ÇÇ &
updateEstimationDocument
ÇÇ (
=
ÇÇ) *
await
ÇÇ+ 0
Mediator
ÇÇ1 9
.
ÇÇ9 :
Send
ÇÇ: >
(
ÇÇ> ?
command
ÇÇ? F
)
ÇÇF G
;
ÇÇG H
return
ÑÑ 
Ok
ÑÑ 
(
ÑÑ &
updateEstimationDocument
ÑÑ .
.
ÑÑ. /
Identity
ÑÑ/ 7
)
ÑÑ7 8
;
ÑÑ8 9
}
ÖÖ 	
[
áá 	

HttpDelete
áá	 
(
áá 
$str
áá 
)
áá 
]
áá 
public
àà 
async
àà 
Task
àà 
<
àà 
IActionResult
àà '
>
àà' (
Delete
àà) /
(
àà/ 0
string
àà0 6
Id
àà7 9
)
àà9 :
{
ââ 	
if
ää 
(
ää 
!
ää 
Guid
ää 
.
ää 
TryParse
ää 
(
ää 
Id
ää !
,
ää! "
out
ää# &
Guid
ää' +

documentId
ää, 6
)
ää6 7
)
ää7 8
{
ãã 
return
åå 
NotFound
åå 
(
åå  
)
åå  !
;
åå! "
}
çç 
var
èè 
command
èè 
=
èè 
new
èè ,
RemoveEstimationProductCommand
èè <
(
èè< =
)
èè= >
;
èè> ?
command
êê 
.
êê 
SetId
êê 
(
êê 

documentId
êê $
)
êê$ %
;
êê% &
var
íí '
deletedEstimationDocument
íí )
=
íí* +
await
íí, 1
Mediator
íí2 :
.
íí: ;
Send
íí; ?
(
íí? @
command
íí@ G
)
ííG H
;
ííH I
return
îî 
Ok
îî 
(
îî '
deletedEstimationDocument
îî /
.
îî/ 0
Identity
îî0 8
)
îî8 9
;
îî9 :
}
ïï 	
}
ññ 
}óó ãï
rD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Controllers\Api\FabricConstructionController.cs
	namespace 	
Manufactures
 
. 
Controllers "
." #
Api# &
{ 
[ 
Produces 
( 
$str  
)  !
]! "
[ 
Route 

(
 
$str )
)) *
]* +
[ 
ApiController 
] 
[ 
	Authorize 
] 
public 

class (
FabricConstructionController -
:. /
ControllerApiBase0 A
{ 
private 
readonly )
IFabricConstructionRepository 6+
_constructionDocumentRepository7 V
;V W
private 
readonly #
IMaterialTypeRepository 0#
_materialTypeRepository1 H
;H I
private 
readonly #
IYarnDocumentRepository 0#
_yarnDocumentRepository1 H
;H I
private 
readonly !
IYarnNumberRepository .!
_yarnNumberRepository/ D
;D E
public (
FabricConstructionController +
(+ ,
IServiceProvider, <
serviceProvider= L
,L M
IWorkContext  & 2
workContext  3 >
)  > ?
:  @ A
base  B F
(  F G
serviceProvider  G V
)  V W
{!! 	+
_constructionDocumentRepository"" +
="", -
this## 
.## 
Storage## 
.## 
GetRepository## *
<##* +)
IFabricConstructionRepository##+ H
>##H I
(##I J
)##J K
;##K L#
_materialTypeRepository$$ #
=$$$ %
this%% 
.%% 
Storage%% 
.%% 
GetRepository%% *
<%%* +#
IMaterialTypeRepository%%+ B
>%%B C
(%%C D
)%%D E
;%%E F#
_yarnDocumentRepository&& #
=&&$ %
this'' 
.'' 
Storage'' 
.'' 
GetRepository'' *
<''* +#
IYarnDocumentRepository''+ B
>''B C
(''C D
)''D E
;''E F!
_yarnNumberRepository(( !
=((" #
this)) 
.)) 
Storage)) 
.)) 
GetRepository)) *
<))* +!
IYarnNumberRepository))+ @
>))@ A
())A B
)))B C
;))C D
}** 	
[,, 	
HttpGet,,	 
],, 
public-- 
async-- 
Task-- 
<-- 
IActionResult-- '
>--' (
Get--) ,
(--, -
int--- 0
page--1 5
=--6 7
$num--8 9
,--9 :
int..- 0
size..1 5
=..6 7
$num..8 :
,..: ;
string//- 3
order//4 9
=//: ;
$str//< @
,//@ A
string00- 3
keyword004 ;
=00< =
null00> B
,00B C
string11- 3
filter114 :
=11; <
$str11= A
)11A B
{22 	
page33 
=33 
page33 
-33 
$num33 
;33 
var44 
query44 
=44 +
_constructionDocumentRepository55 /
.55/ 0
Query550 5
.555 6
OrderByDescending556 G
(55G H
item55H L
=>55M O
item55P T
.55T U
CreatedDate55U `
)55` a
;55a b
var66 !
constructionDocuments66 %
=66& '+
_constructionDocumentRepository77 /
.77/ 0
Find770 4
(774 5
query775 :
)77: ;
.88/ 0
Select880 6
(886 7
item887 ;
=>88< >
new88? B)
FabricConstructionDocumentDto88C `
(88` a
item88a e
)88e f
)88f g
;88g h
if:: 
(:: 
!:: 
string:: 
.:: 
IsNullOrEmpty:: %
(::% &
keyword::& -
)::- .
)::. /
{;; !
constructionDocuments<< %
=<<& '!
constructionDocuments== )
.>> 
Where>> 
(>> 
entity>> %
=>>>& (
entity>>) /
.>>/ 0
ConstructionNumber>>0 B
.>>B C
Contains>>C K
(>>K L
keyword>>L S
,>>S T
StringComparison??L \
.??\ ]
OrdinalIgnoreCase??] n
)??n o
)??o p
;??p q
}@@ 
ifBB 
(BB 
!BB 
orderBB 
.BB 
ContainsBB 
(BB  
$strBB  $
)BB$ %
)BB% &
{CC 

DictionaryDD 
<DD 
stringDD !
,DD! "
stringDD# )
>DD) *
orderDictionaryDD+ :
=DD; <
JsonConvertEE 
.EE  
DeserializeObjectEE  1
<EE1 2

DictionaryEE2 <
<EE< =
stringEE= C
,EEC D
stringEEE K
>EEK L
>EEL M
(EEM N
orderEEN S
)EES T
;EET U
varFF 
keyFF 
=FF 
orderDictionaryFF )
.FF) *
KeysFF* .
.FF. /
FirstFF/ 4
(FF4 5
)FF5 6
.FF6 7
	SubstringFF7 @
(FF@ A
$numFFA B
,FFB C
$numFFD E
)FFE F
.FFF G
ToUpperFFG N
(FFN O
)FFO P
+FFQ R
orderDictionaryGG )
.GG) *
KeysGG* .
.GG. /
FirstGG/ 4
(GG4 5
)GG5 6
.GG6 7
	SubstringGG7 @
(GG@ A
$numGGA B
)GGB C
;GGC D
SystemHH 
.HH 

ReflectionHH !
.HH! "
PropertyInfoHH" .
propHH/ 3
=HH4 5
typeofHH6 <
(HH< =)
FabricConstructionDocumentDtoHH= Z
)HHZ [
.HH[ \
GetPropertyHH\ g
(HHg h
keyHHh k
)HHk l
;HHl m
ifJJ 
(JJ 
orderDictionaryJJ #
.JJ# $
ValuesJJ$ *
.JJ* +
ContainsJJ+ 3
(JJ3 4
$strJJ4 9
)JJ9 :
)JJ: ;
{KK !
constructionDocumentsLL )
=LL* +!
constructionDocumentsMM -
.MM- .
OrderByMM. 5
(MM5 6
xMM6 7
=>MM8 :
propMM; ?
.MM? @
GetValueMM@ H
(MMH I
xMMI J
,MMJ K
nullMML P
)MMP Q
)MMQ R
;MMR S
}NN 
elseOO 
{PP !
constructionDocumentsQQ )
=QQ* +!
constructionDocumentsRR -
.RR- .
OrderByDescendingRR. ?
(RR? @
xRR@ A
=>RRB D
propRRE I
.RRI J
GetValueRRJ R
(RRR S
xRRS T
,RRT U
nullRRV Z
)RRZ [
)RR[ \
;RR\ ]
}SS 
}TT !
constructionDocumentsVV !
=VV" #!
constructionDocumentsWW %
.WW% &
SkipWW& *
(WW* +
pageWW+ /
*WW0 1
sizeWW2 6
)WW6 7
.WW7 8
TakeWW8 <
(WW< =
sizeWW= A
)WWA B
;WWB C
intXX 
	totalRowsXX 
=XX !
constructionDocumentsXX 1
.XX1 2
CountXX2 7
(XX7 8
)XX8 9
;XX9 :
pageYY 
=YY 
pageYY 
+YY 
$numYY 
;YY 
await[[ 
Task[[ 
.[[ 
Yield[[ 
([[ 
)[[ 
;[[ 
return]] 
Ok]] 
(]] !
constructionDocuments]] +
,]]+ ,
info]]- 1
:]]1 2
new]]3 6
{^^ 
page__ 
,__ 
size`` 
,`` 
totalaa 
=aa 
	totalRowsaa !
}bb 
)bb 
;bb 
}cc 	
[ee 	
HttpGetee	 
(ee 
$stree 
)ee 
]ee 
publicff 
asyncff 
Taskff 
<ff 
IActionResultff '
>ff' (
Getff) ,
(ff, -
stringff- 3
Idff4 6
)ff6 7
{gg 	
varhh 
Identityhh 
=hh 
Guidhh 
.hh  
Parsehh  %
(hh% &
Idhh& (
)hh( )
;hh) *
varii  
constructionDocumentii $
=ii% &+
_constructionDocumentRepositoryjj /
.jj/ 0
Findjj0 4
(jj4 5
ojj5 6
=>jj7 9
ojj: ;
.jj; <
Identityjj< D
==jjE G
IdentityjjH P
)jjP Q
.kk/ 0
FirstOrDefaultkk0 >
(kk> ?
)kk? @
;kk@ A
varmm 
resultmm 
=mm 
newmm %
FabricConstructionByIdDtomm 6
(mm6 7 
constructionDocumentmm7 K
)mmK L
;mmL M
foreachoo 
(oo 
varoo 
detailoo 
inoo ! 
constructionDocumentoo" 6
.oo6 7

ListOfWarpoo7 A
)ooA B
{pp 
varqq 
yarnqq 
=qq #
_yarnDocumentRepositoryrr +
.rr+ ,
Findrr, 0
(rr0 1
orr1 2
=>rr3 5
orr6 7
.rr7 8
Identityrr8 @
==rrA C
detailrrD J
.rrJ K
YarnIdrrK Q
.rrQ R
ValuerrR W
)rrW X
.ss+ ,
FirstOrDefaultss, :
(ss: ;
)ss; <
;ss< =
vartt 
materialTypett  
=tt! "#
_materialTypeRepositoryuu +
.uu+ ,
Finduu, 0
(uu0 1
ouu1 2
=>uu3 5
ouu6 7
.uu7 8
Identityuu8 @
==uuA C
yarnuuD H
.uuH I
MaterialTypeIduuI W
.uuW X
ValueuuX ]
)uu] ^
.vv+ ,
FirstOrDefaultvv, :
(vv: ;
)vv; <
;vv< =
varww 

yarnNumberww 
=ww  !
_yarnNumberRepositoryxx )
.xx) *
Findxx* .
(xx. /
oxx/ 0
=>xx1 3
oxx4 5
.xx5 6
Identityxx6 >
==xx? A
yarnxxB F
.xxF G
YarnNumberIdxxG S
.xxS T
ValuexxT Y
)xxY Z
.yy) *
FirstOrDefaultyy* 8
(yy8 9
)yy9 :
;yy: ;
var{{ 
yarnValueObject{{ #
={{$ %
new|| 
YarnValueObject|| '
(||' (
yarn||( ,
.||, -
Identity||- 5
,||5 6
yarn}}( ,
.}}, -
Code}}- 1
,}}1 2
yarn~~( ,
.~~, -
Name~~- 1
,~~1 2
materialType( 4
.4 5
Code5 9
,9 :

yarnNumber
ÄÄ( 2
.
ÄÄ2 3
Code
ÄÄ3 7
)
ÄÄ7 8
;
ÄÄ8 9
var
ÇÇ 
warp
ÇÇ 
=
ÇÇ 
new
ÉÉ 
Warp
ÉÉ 
(
ÉÉ 
detail
ÉÉ #
.
ÉÉ# $
Quantity
ÉÉ$ ,
,
ÉÉ, -
detail
ÑÑ #
.
ÑÑ# $
Information
ÑÑ$ /
,
ÑÑ/ 0
yarnValueObject
ÖÖ ,
)
ÖÖ, -
;
ÖÖ- .
result
áá 
.
áá 
AddWarp
áá 
(
áá 
warp
áá #
)
áá# $
;
áá$ %
await
àà 
Task
àà 
.
àà 
Yield
àà  
(
àà  !
)
àà! "
;
àà" #
}
ââ 
foreach
ãã 
(
ãã 
var
ãã 
detail
ãã 
in
ãã !"
constructionDocument
ãã" 6
.
ãã6 7

ListOfWeft
ãã7 A
)
ããA B
{
åå 
var
çç 
yarn
çç 
=
çç %
_yarnDocumentRepository
çç 2
.
çç2 3
Find
çç3 7
(
çç7 8
o
çç8 9
=>
çç: <
o
çç= >
.
çç> ?
Identity
çç? G
==
ççH J
detail
ççK Q
.
ççQ R
YarnId
ççR X
.
ççX Y
Value
ççY ^
)
çç^ _
.
çç_ `
FirstOrDefault
çç` n
(
ççn o
)
çço p
;
ççp q
var
éé 
materialType
éé  
=
éé! "%
_materialTypeRepository
éé# :
.
éé: ;
Find
éé; ?
(
éé? @
o
éé@ A
=>
ééB D
o
ééE F
.
ééF G
Identity
ééG O
==
ééP R
yarn
ééS W
.
ééW X
MaterialTypeId
ééX f
.
ééf g
Value
éég l
)
éél m
.
éém n
FirstOrDefault
één |
(
éé| }
)
éé} ~
;
éé~ 
var
èè 

yarnNumber
èè 
=
èè  #
_yarnNumberRepository
èè! 6
.
èè6 7
Find
èè7 ;
(
èè; <
o
èè< =
=>
èè> @
o
èèA B
.
èèB C
Identity
èèC K
==
èèL N
yarn
èèO S
.
èèS T
YarnNumberId
èèT `
.
èè` a
Value
èèa f
)
èèf g
.
èèg h
FirstOrDefault
èèh v
(
èèv w
)
èèw x
;
èèx y
var
ëë 
yarnValueObject
ëë #
=
ëë$ %
new
ëë& )
YarnValueObject
ëë* 9
(
ëë9 :
yarn
ëë: >
.
ëë> ?
Identity
ëë? G
,
ëëG H
yarn
ëëI M
.
ëëM N
Code
ëëN R
,
ëëR S
yarn
ëëT X
.
ëëX Y
Name
ëëY ]
,
ëë] ^
materialType
ëë_ k
.
ëëk l
Code
ëël p
,
ëëp q

yarnNumber
ëër |
.
ëë| }
Codeëë} Å
)ëëÅ Ç
;ëëÇ É
var
ìì 
weft
ìì 
=
ìì 
new
ìì 
Weft
ìì #
(
ìì# $
detail
ìì$ *
.
ìì* +
Quantity
ìì+ 3
,
ìì3 4
detail
ìì5 ;
.
ìì; <
Information
ìì< G
,
ììG H
yarnValueObject
ììI X
)
ììX Y
;
ììY Z
result
ïï 
.
ïï 
AddWeft
ïï 
(
ïï 
weft
ïï #
)
ïï# $
;
ïï$ %
await
ññ 
Task
ññ 
.
ññ 
Yield
ññ  
(
ññ  !
)
ññ! "
;
ññ" #
}
óó 
await
ôô 
Task
ôô 
.
ôô 
Yield
ôô 
(
ôô 
)
ôô 
;
ôô 
if
õõ 
(
õõ "
constructionDocument
õõ $
==
õõ% '
null
õõ( ,
)
õõ, -
{
úú 
return
ùù 
NotFound
ùù 
(
ùù  
)
ùù  !
;
ùù! "
}
ûû 
else
üü 
{
†† 
return
°° 
Ok
°° 
(
°° 
result
°°  
)
°°  !
;
°°! "
}
¢¢ 
}
££ 	
[
•• 	
HttpPost
••	 
]
•• 
public
¶¶ 
async
¶¶ 
Task
¶¶ 
<
¶¶ 
IActionResult
¶¶ '
>
¶¶' (
Post
¶¶) -
(
¶¶- .
[
¶¶. /
FromBody
¶¶/ 7
]
¶¶7 8*
AddFabricConstructionCommand
¶¶8 T
command
¶¶U \
)
¶¶\ ]
{
ßß 	
var
®® %
newConstructionDocument
®® '
=
®®( )
await
®®* /
Mediator
®®0 8
.
®®8 9
Send
®®9 =
(
®®= >
command
®®> E
)
®®E F
;
®®F G
return
™™ 
Ok
™™ 
(
™™ %
newConstructionDocument
™™ -
.
™™- .
Identity
™™. 6
)
™™6 7
;
™™7 8
}
´´ 	
[
≠≠ 	
HttpPut
≠≠	 
(
≠≠ 
$str
≠≠ 
)
≠≠ 
]
≠≠ 
public
ÆÆ 
async
ÆÆ 
Task
ÆÆ 
<
ÆÆ 
IActionResult
ÆÆ '
>
ÆÆ' (
Put
ÆÆ) ,
(
ÆÆ, -
string
ÆÆ- 3
Id
ÆÆ4 6
,
ÆÆ6 7
[
ØØ- .
FromBody
ØØ. 6
]
ØØ6 7-
UpdateFabricConstructionCommand
ØØ7 V
command
ØØW ^
)
ØØ^ _
{
∞∞ 	
if
±± 
(
±± 
!
±± 
Guid
±± 
.
±± 
TryParse
±± 
(
±± 
Id
±± !
,
±±! "
out
±±# &
Guid
±±' +

documentId
±±, 6
)
±±6 7
)
±±7 8
{
≤≤ 
return
≥≥ 
NotFound
≥≥ 
(
≥≥  
)
≥≥  !
;
≥≥! "
}
¥¥ 
command
∂∂ 
.
∂∂ 
SetId
∂∂ 
(
∂∂ 

documentId
∂∂ $
)
∂∂$ %
;
∂∂% &
var
∑∑ (
updateConstructionDocument
∑∑ *
=
∑∑+ ,
await
∑∑- 2
Mediator
∑∑3 ;
.
∑∑; <
Send
∑∑< @
(
∑∑@ A
command
∑∑A H
)
∑∑H I
;
∑∑I J
return
ππ 
Ok
ππ 
(
ππ (
updateConstructionDocument
ππ 0
.
ππ0 1
Identity
ππ1 9
)
ππ9 :
;
ππ: ;
}
∫∫ 	
[
ºº 	

HttpDelete
ºº	 
(
ºº 
$str
ºº 
)
ºº 
]
ºº 
public
ΩΩ 
async
ΩΩ 
Task
ΩΩ 
<
ΩΩ 
IActionResult
ΩΩ '
>
ΩΩ' (
Delete
ΩΩ) /
(
ΩΩ/ 0
string
ΩΩ0 6
Id
ΩΩ7 9
)
ΩΩ9 :
{
ææ 	
if
øø 
(
øø 
!
øø 
Guid
øø 
.
øø 
TryParse
øø 
(
øø 
Id
øø !
,
øø! "
out
øø# &
Guid
øø' +

documentId
øø, 6
)
øø6 7
)
øø7 8
{
¿¿ 
return
¡¡ 
NotFound
¡¡ 
(
¡¡  
)
¡¡  !
;
¡¡! "
}
¬¬ 
var
ƒƒ 
command
ƒƒ 
=
ƒƒ 
new
ƒƒ -
RemoveFabricConstructionCommand
ƒƒ =
(
ƒƒ= >
)
ƒƒ> ?
;
ƒƒ? @
command
≈≈ 
.
≈≈ 
SetId
≈≈ 
(
≈≈ 

documentId
≈≈ $
)
≈≈$ %
;
≈≈% &
var
«« )
deletedConstructionDocument
«« +
=
««, -
await
««. 3
Mediator
««4 <
.
««< =
Send
««= A
(
««A B
command
««B I
)
««I J
;
««J K
return
…… 
Ok
…… 
(
…… )
deletedConstructionDocument
…… 1
.
……1 2
Identity
……2 :
)
……: ;
;
……; <
}
   	
}
ÀÀ 
}ÃÃ °]
oD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Controllers\Api\MachineDocumentController.cs
	namespace 	
Manufactures
 
. 
Controllers "
." #
Api# &
{ 
[ 
Produces 
( 
$str  
)  !
]! "
[ 
Route 

(
 
$str 
) 
] 
[ 
ApiController 
] 
[ 
	Authorize 
] 
public 

class %
MachineDocumentController *
:+ ,
ControllerApiBase- >
{ 
private 
readonly 
IMachineRepository +
_machineRepository, >
;> ?
public %
MachineDocumentController (
(( )
IServiceProvider) 9
serviceProvider: I
)I J
: 
base 
( 
serviceProvider "
)" #
{ 	
_machineRepository 
=  
this 
. 
Storage 
. 
GetRepository *
<* +
IMachineRepository+ =
>= >
(> ?
)? @
;@ A
} 	
[ 	
HttpGet	 
] 
public 
async 
Task 
< 
IActionResult '
>' (
Get) ,
(, -
int- 0
page1 5
=6 7
$num8 9
,9 :
int  - 0
size  1 5
=  6 7
$num  8 :
,  : ;
string!!- 3
order!!4 9
=!!: ;
$str!!< @
,!!@ A
string""- 3
keyword""4 ;
=""< =
null""> B
,""B C
string##- 3
filter##4 :
=##; <
$str##= A
)##A B
{$$ 	
page%% 
=%% 
page%% 
-%% 
$num%% 
;%% 
var&& 
query&& 
=&& 
_machineRepository'' "
.''" #
Query''# (
.''( )
OrderByDescending'') :
('': ;
item''; ?
=>''@ B
item''C G
.''G H
CreatedDate''H S
)''S T
;''T U
var(( 
machine(( 
=(( 
_machineRepository)) "
.))" #
Find))# '
())' (
query))( -
)))- .
.**' (
Select**( .
(**. /
item**/ 3
=>**4 6
new**7 :
MachineListDto**; I
(**I J
item**J N
)**N O
)**O P
;**P Q
if,, 
(,, 
!,, 
string,, 
.,, 
IsNullOrEmpty,, %
(,,% &
keyword,,& -
),,- .
),,. /
{-- 
machine.. 
=.. 
machine// 
.// 
Where// !
(//! "
entity//" (
=>//) +
entity//, 2
.//2 3
MachineNumber//3 @
.//@ A
Contains//A I
(//I J
keyword//J Q
,//Q R
StringComparison00J Z
.00Z [
OrdinalIgnoreCase00[ l
)00l m
||00n p
entity11, 2
.112 3
Location113 ;
.11; <
Contains11< D
(11D E
keyword11E L
,11L M
StringComparison22E U
.22U V
OrdinalIgnoreCase22V g
)22g h
)22h i
;22i j
}33 
if55 
(55 
!55 
order55 
.55 
Contains55 
(55  
$str55  $
)55$ %
)55% &
{66 

Dictionary77 
<77 
string77 !
,77! "
string77# )
>77) *
orderDictionary77+ :
=77; <
JsonConvert88 
.88  
DeserializeObject88  1
<881 2

Dictionary882 <
<88< =
string88= C
,88C D
string88E K
>88K L
>88L M
(88M N
order88N S
)88S T
;88T U
var99 
key99 
=99 
orderDictionary99 )
.99) *
Keys99* .
.99. /
First99/ 4
(994 5
)995 6
.996 7
	Substring997 @
(99@ A
$num99A B
,99B C
$num99D E
)99E F
.99F G
ToUpper99G N
(99N O
)99O P
+99Q R
orderDictionary:: )
.::) *
Keys::* .
.::. /
First::/ 4
(::4 5
)::5 6
.::6 7
	Substring::7 @
(::@ A
$num::A B
)::B C
;::C D
System;; 
.;; 

Reflection;; !
.;;! "
PropertyInfo;;" .
prop;;/ 3
=;;4 5
typeof;;6 <
(;;< =
MachineListDto;;= K
);;K L
.;;L M
GetProperty;;M X
(;;X Y
key;;Y \
);;\ ]
;;;] ^
if== 
(== 
orderDictionary== #
.==# $
Values==$ *
.==* +
Contains==+ 3
(==3 4
$str==4 9
)==9 :
)==: ;
{>> 
machine?? 
=?? 
machine?? %
.??% &
OrderBy??& -
(??- .
x??. /
=>??0 2
prop??3 7
.??7 8
GetValue??8 @
(??@ A
x??A B
,??B C
null??D H
)??H I
)??I J
;??J K
}@@ 
elseAA 
{BB 
machineCC 
=CC 
machineCC %
.CC% &
OrderByDescendingCC& 7
(CC7 8
xCC8 9
=>CC: <
propCC= A
.CCA B
GetValueCCB J
(CCJ K
xCCK L
,CCL M
nullCCN R
)CCR S
)CCS T
;CCT U
}DD 
}EE 
machineGG 
=GG 
machineGG 
.GG 
SkipGG "
(GG" #
pageGG# '
*GG( )
sizeGG* .
)GG. /
.GG/ 0
TakeGG0 4
(GG4 5
sizeGG5 9
)GG9 :
;GG: ;
intHH 
	totalRowsHH 
=HH 
machineHH #
.HH# $
CountHH$ )
(HH) *
)HH* +
;HH+ ,
pageII 
=II 
pageII 
+II 
$numII 
;II 
awaitKK 
TaskKK 
.KK 
YieldKK 
(KK 
)KK 
;KK 
returnMM 
OkMM 
(MM 
machineMM 
,MM 
infoMM #
:MM# $
newMM% (
{NN 
pageOO 
,OO 
sizePP 
,PP 
totalQQ 
=QQ 
	totalRowsQQ !
}RR 
)RR 
;RR 
}SS 	
[UU 	
HttpGetUU	 
(UU 
$strUU 
)UU 
]UU 
publicVV 
asyncVV 
TaskVV 
<VV 
IActionResultVV '
>VV' (
GetVV) ,
(VV, -
stringVV- 3
IdVV4 6
)VV6 7
{WW 	
varXX 
IdentityXX 
=XX 
GuidXX 
.XX  
ParseXX  %
(XX% &
IdXX& (
)XX( )
;XX) *
varYY 
machineYY 
=YY 
_machineRepositoryZZ "
.ZZ" #
FindZZ# '
(ZZ' (
itemZZ( ,
=>ZZ- /
itemZZ0 4
.ZZ4 5
IdentityZZ5 =
==ZZ> @
IdentityZZA I
)ZZI J
.[[" #
FirstOrDefault[[# 1
([[1 2
)[[2 3
;[[3 4
await]] 
Task]] 
.]] 
Yield]] 
(]] 
)]] 
;]] 
if__ 
(__ 
machine__ 
==__ 
null__ 
)__  
{`` 
returnaa 
NotFoundaa 
(aa  
)aa  !
;aa! "
}bb 
elsecc 
{dd 
varee 
resultee 
=ee 
newee  
MachineDocumentDtoee! 3
(ee3 4
machineee4 ;
)ee; <
;ee< =
returnff 
Okff 
(ff 
resultff  
)ff  !
;ff! "
}gg 
}hh 	
[jj 	
HttpPostjj	 
]jj 
publickk 
asynckk 
Taskkk 
<kk 
IActionResultkk '
>kk' (
Postkk) -
(kk- .
[kk. /
FromBodykk/ 7
]kk7 8 
AddNewMachineCommandkk8 L
commandkkM T
)kkT U
{ll 	
varmm 
machinemm 
=mm 
awaitmm 
Mediatormm  (
.mm( )
Sendmm) -
(mm- .
commandmm. 5
)mm5 6
;mm6 7
returnoo 
Okoo 
(oo 
machineoo 
.oo 
Identityoo &
)oo& '
;oo' (
}pp 	
[rr 	
HttpPutrr	 
(rr 
$strrr 
)rr 
]rr 
publicss 
asyncss 
Taskss 
<ss 
IActionResultss '
>ss' (
Putss) ,
(ss, -
stringss- 3
Idss4 6
,ss6 7
[tt- .
FromBodytt. 6
]tt6 7(
UpdateExistingMachineCommandtt7 S
commandttT [
)tt[ \
{uu 	
ifvv 
(vv 
!vv 
Guidvv 
.vv 
TryParsevv 
(vv 
Idvv !
,vv! "
outvv# &
Guidvv' +
Identityvv, 4
)vv4 5
)vv5 6
{ww 
returnxx 
NotFoundxx 
(xx  
)xx  !
;xx! "
}yy 
command{{ 
.{{ 
SetId{{ 
({{ 
Identity{{ "
){{" #
;{{# $
var|| 
machine|| 
=|| 
await|| 
Mediator||  (
.||( )
Send||) -
(||- .
command||. 5
)||5 6
;||6 7
return~~ 
Ok~~ 
(~~ 
machine~~ 
.~~ 
Identity~~ &
)~~& '
;~~' (
} 	
[
ÅÅ 	

HttpDelete
ÅÅ	 
(
ÅÅ 
$str
ÅÅ 
)
ÅÅ 
]
ÅÅ 
public
ÇÇ 
async
ÇÇ 
Task
ÇÇ 
<
ÇÇ 
IActionResult
ÇÇ '
>
ÇÇ' (
Delete
ÇÇ) /
(
ÇÇ/ 0
string
ÇÇ0 6
Id
ÇÇ7 9
)
ÇÇ9 :
{
ÉÉ 	
if
ÑÑ 
(
ÑÑ 
!
ÑÑ 
Guid
ÑÑ 
.
ÑÑ 
TryParse
ÑÑ 
(
ÑÑ 
Id
ÑÑ !
,
ÑÑ! "
out
ÑÑ# &
Guid
ÑÑ' +
Identity
ÑÑ, 4
)
ÑÑ4 5
)
ÑÑ5 6
{
ÖÖ 
return
ÜÜ 
NotFound
ÜÜ 
(
ÜÜ  
)
ÜÜ  !
;
ÜÜ! "
}
áá 
var
ââ 
command
ââ 
=
ââ 
new
ââ *
RemoveExistingMachineCommand
ââ :
(
ââ: ;
)
ââ; <
;
ââ< =
command
ää 
.
ää 
SetId
ää 
(
ää 
Identity
ää "
)
ää" #
;
ää# $
var
åå 
machine
åå 
=
åå 
await
åå 
Mediator
åå  (
.
åå( )
Send
åå) -
(
åå- .
command
åå. 5
)
åå5 6
;
åå6 7
return
éé 
Ok
éé 
(
éé 
machine
éé 
.
éé 
Identity
éé &
)
éé& '
;
éé' (
}
èè 	
}
êê 
}ëë ôí
pD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Controllers\Api\MachinesPlanningController.cs
	namespace 	
Manufactures
 
. 
Controllers "
." #
Api# &
{ 
[ 
Produces 
( 
$str  
)  !
]! "
[ 
Route 

(
 
$str &
)& '
]' (
[ 
ApiController 
] 
[ 
	Authorize 
] 
public 

class &
MachinesPlanningController +
:, -
ControllerApiBase. ?
{ 
private 
readonly '
IMachinesPlanningRepository 4'
_machinesPlanningRepository5 P
;P Q
private 
readonly 
IMachineRepository +
_machineRepository, >
;> ?
private 
readonly "
IMachineTypeRepository /"
_machineTypeRepository0 F
;F G
public &
MachinesPlanningController )
() *
IServiceProvider* :
serviceProvider; J
)J K
: 
base 
( 
serviceProvider "
)" #
{ 	'
_machinesPlanningRepository   '
=  ( )
this!! 
.!! 
Storage!! 
.!! 
GetRepository!! *
<!!* +'
IMachinesPlanningRepository!!+ F
>!!F G
(!!G H
)!!H I
;!!I J
_machineRepository"" 
=""  
this## 
.## 
Storage## 
.## 
GetRepository## *
<##* +
IMachineRepository##+ =
>##= >
(##> ?
)##? @
;##@ A"
_machineTypeRepository$$ "
=$$# $
this%% 
.%% 
Storage%% 
.%% 
GetRepository%% +
<%%+ ,"
IMachineTypeRepository%%, B
>%%B C
(%%C D
)%%D E
;%%E F
}&& 	
[(( 	
HttpGet((	 
](( 
public)) 
async)) 
Task)) 
<)) 
IActionResult)) '
>))' (
Get))) ,
()), -
int))- 0
page))1 5
=))6 7
$num))8 9
,))9 :
int**, /
size**0 4
=**5 6
$num**7 9
,**9 :
string++, 2
order++3 8
=++9 :
$str++; ?
,++? @
string,,, 2
keyword,,3 :
=,,; <
null,,= A
,,,A B
string--, 2
filter--3 9
=--: ;
$str--< @
)--@ A
{.. 	
page// 
=// 
page// 
-// 
$num// 
;// 
var00 
query00 
=00 '
_machinesPlanningRepository11 +
.11+ ,
Query11, 1
.111 2
OrderByDescending112 C
(11C D
item11D H
=>11I K
item11L P
.11P Q
CreatedDate11Q \
)11\ ]
;11] ^
var22 
machinesPlanning22  
=22! "'
_machinesPlanningRepository33 +
.33+ ,
Find33, 0
(330 1
query331 6
)336 7
;337 8
var55 
machineDtos55 
=55 
new55 !
List55" &
<55& '#
MachinesPlanningListDto55' >
>55> ?
(55? @
)55@ A
;55A B
foreach77 
(77 
var77 
machinePlanning77 (
in77) +
machinesPlanning77, <
)77< =
{88 
var99  
manufacturingMachine99 (
=99) *
_machineRepository:: &
.::& '
Find::' +
(::+ ,
item::, 0
=>::1 3
item::4 8
.::8 9
Identity::9 A
==::B D
machinePlanning::E T
.::T U
	MachineId::U ^
.::^ _
Value::_ d
)::d e
.;;& '
FirstOrDefault;;' 5
(;;5 6
);;6 7
;;;7 8
var<< $
manufacturingMachineType<< ,
=<<- ."
_machineTypeRepository== *
.==* +
Find==+ /
(==/ 0
item==0 4
=>==5 7
item==8 <
.==< =
Identity=== E
====F H 
manufacturingMachine==I ]
.==] ^
MachineTypeId==^ k
.==k l
Value==l q
)==q r
.>>* +
FirstOrDefault>>+ 9
(>>9 :
)>>: ;
;>>; <
var@@ 
machine@@ 
=@@ 
new@@ !
ManufactureMachine@@" 4
(@@4 5 
manufacturingMachine@@5 I
,@@I J$
manufacturingMachineType@@K c
)@@c d
;@@d e
varAA 

machineDtoAA 
=AA  
newAA! $#
MachinesPlanningListDtoAA% <
(AA< =
machinePlanningAA= L
,AAL M
machineAAN U
)AAU V
;AAV W
machineDtosCC 
.CC 
AddCC 
(CC  

machineDtoCC  *
)CC* +
;CC+ ,
}DD 
ifGG 
(GG 
!GG 
stringGG 
.GG 
IsNullOrEmptyGG %
(GG% &
keywordGG& -
)GG- .
)GG. /
{HH 
machineDtosII 
=II 
machineDtosJJ 
.JJ  
WhereJJ  %
(JJ% &
entityJJ& ,
=>JJ- /
entityJJ0 6
.JJ6 7
AreaJJ7 ;
.JJ; <
ContainsJJ< D
(JJD E
keywordJJE L
,JJL M
StringComparisonJJN ^
.JJ^ _
OrdinalIgnoreCaseJJ_ p
)JJp q
||JJr t
entityKK0 6
.KK6 7
BlokKK7 ;
.KK; <
ContainsKK< D
(KKD E
keywordKKE L
,KKL M
StringComparisonKKN ^
.KK^ _
OrdinalIgnoreCaseKK_ p
)KKp q
||KKr t
entityLL0 6
.LL6 7

BlokKaizenLL7 A
.LLA B
ContainsLLB J
(LLJ K
keywordLLK R
,LLR S
StringComparisonLLT d
.LLd e
OrdinalIgnoreCaseLLe v
)LLv w
)LLw x
.MM  
ToListMM  &
(MM& '
)MM' (
;MM( )
}NN 
ifPP 
(PP 
!PP 
orderPP 
.PP 
ContainsPP 
(PP  
$strPP  $
)PP$ %
)PP% &
{QQ 

DictionaryRR 
<RR 
stringRR !
,RR! "
stringRR# )
>RR) *
orderDictionaryRR+ :
=RR; <
JsonConvertSS 
.SS  
DeserializeObjectSS  1
<SS1 2

DictionarySS2 <
<SS< =
stringSS= C
,SSC D
stringSSE K
>SSK L
>SSL M
(SSM N
orderSSN S
)SSS T
;SST U
varTT 
keyTT 
=TT 
orderDictionaryTT )
.TT) *
KeysTT* .
.TT. /
FirstTT/ 4
(TT4 5
)TT5 6
.TT6 7
	SubstringTT7 @
(TT@ A
$numTTA B
,TTB C
$numTTD E
)TTE F
.TTF G
ToUpperTTG N
(TTN O
)TTO P
+TTQ R
orderDictionaryUU )
.UU) *
KeysUU* .
.UU. /
FirstUU/ 4
(UU4 5
)UU5 6
.UU6 7
	SubstringUU7 @
(UU@ A
$numUUA B
)UUB C
;UUC D
SystemVV 
.VV 

ReflectionVV !
.VV! "
PropertyInfoVV" .
propVV/ 3
=VV4 5
typeofVV6 <
(VV< =#
MachinesPlanningListDtoVV= T
)VVT U
.VVU V
GetPropertyVVV a
(VVa b
keyVVb e
)VVe f
;VVf g
ifXX 
(XX 
orderDictionaryXX #
.XX# $
ValuesXX$ *
.XX* +
ContainsXX+ 3
(XX3 4
$strXX4 9
)XX9 :
)XX: ;
{YY 
machineDtosZZ 
=ZZ  !
machineDtosZZ" -
.ZZ- .
OrderByZZ. 5
(ZZ5 6
xZZ6 7
=>ZZ8 :
propZZ; ?
.ZZ? @
GetValueZZ@ H
(ZZH I
xZZI J
,ZZJ K
nullZZL P
)ZZP Q
)ZZQ R
.ZZR S
ToListZZS Y
(ZZY Z
)ZZZ [
;ZZ[ \
}[[ 
else\\ 
{]] 
machineDtos^^ 
=^^  !
machineDtos^^" -
.^^- .
OrderByDescending^^. ?
(^^? @
x^^@ A
=>^^B D
prop^^E I
.^^I J
GetValue^^J R
(^^R S
x^^S T
,^^T U
null^^V Z
)^^Z [
)^^[ \
.^^\ ]
ToList^^] c
(^^c d
)^^d e
;^^e f
}__ 
}`` 
machineDtosbb 
=bb 
machineDtosbb %
.bb% &
Skipbb& *
(bb* +
pagebb+ /
*bb0 1
sizebb2 6
)bb6 7
.bb7 8
Takebb8 <
(bb< =
sizebb= A
)bbA B
.bbB C
ToListbbC I
(bbI J
)bbJ K
;bbK L
intcc 
	totalRowscc 
=cc 
machineDtoscc '
.cc' (
Countcc( -
(cc- .
)cc. /
;cc/ 0
pagedd 
=dd 
pagedd 
+dd 
$numdd 
;dd 
awaitff 
Taskff 
.ff 
Yieldff 
(ff 
)ff 
;ff 
returnhh 
Okhh 
(hh 
machineDtoshh !
,hh! "
infohh# '
:hh' (
newhh) ,
{ii 
pagejj 
,jj 
sizekk 
,kk 
totalll 
=ll 
	totalRowsll !
}mm 
)mm 
;mm 
}nn 	
[pp 	
HttpGetpp	 
(pp 
$strpp 
)pp 
]pp 
publicqq 
asyncqq 
Taskqq 
<qq 
IActionResultqq '
>qq' (
Getqq) ,
(qq, -
stringqq- 3
Idqq4 6
)qq6 7
{rr 	
varss 
Identityss 
=ss 
Guidss 
.ss  
Parsess  %
(ss% &
Idss& (
)ss( )
;ss) *
vartt 
machinePlanningtt 
=tt  !'
_machinesPlanningRepositoryuu +
.uu+ ,
Finduu, 0
(uu0 1
itemuu1 5
=>uu6 8
itemuu9 =
.uu= >
Identityuu> F
==uuG I
IdentityuuJ R
)uuR S
.uuS T
FirstOrDefaultuuT b
(uub c
)uuc d
;uud e
varww  
manufacturingMachineww $
=ww% &
_machineRepositoryxx "
.xx" #
Findxx# '
(xx' (
itemxx( ,
=>xx- /
itemxx0 4
.xx4 5
Identityxx5 =
==xx> @
machinePlanningxxA P
.xxP Q
	MachineIdxxQ Z
.xxZ [
Valuexx[ `
)xx` a
.yy" #
FirstOrDefaultyy# 1
(yy1 2
)yy2 3
;yy3 4
varzz $
manufacturingMachineTypezz (
=zz) *"
_machineTypeRepository{{ &
.{{& '
Find{{' +
({{+ ,
item{{, 0
=>{{1 3
item{{4 8
.{{8 9
Identity{{9 A
=={{B D 
manufacturingMachine{{E Y
.{{Y Z
MachineTypeId{{Z g
.{{g h
Value{{h m
){{m n
.||& '
FirstOrDefault||' 5
(||5 6
)||6 7
;||7 8
var~~ 
machine~~ 
=~~ 
new~~ 
ManufactureMachine~~ 0
(~~0 1 
manufacturingMachine~~1 E
,~~E F$
manufacturingMachineType~~G _
)~~_ `
;~~` a
var 
	resultDto 
= 
new '
MachinesPlanningDocumentDto  ;
(; <
machinePlanning< K
,K L
machineM T
)T U
;U V
await
ÅÅ 
Task
ÅÅ 
.
ÅÅ 
Yield
ÅÅ 
(
ÅÅ 
)
ÅÅ 
;
ÅÅ 
if
ÉÉ 
(
ÉÉ 
	resultDto
ÉÉ 
==
ÉÉ 
null
ÉÉ !
)
ÉÉ! "
{
ÑÑ 
return
ÖÖ 
NotFound
ÖÖ 
(
ÖÖ  
)
ÖÖ  !
;
ÖÖ! "
}
ÜÜ 
else
áá 
{
àà 
return
ââ 
Ok
ââ 
(
ââ 
	resultDto
ââ #
)
ââ# $
;
ââ$ %
}
ää 
}
ãã 	
[
çç 	
HttpPost
çç	 
]
çç 
public
éé 
async
éé 
Task
éé 
<
éé 
IActionResult
éé '
>
éé' (
Post
éé) -
(
éé- .
[
éé. /
FromBody
éé/ 7
]
éé7 8)
AddNewEnginePlanningCommand
éé8 S
command
ééT [
)
éé[ \
{
èè 	
var
êê %
existingMachinePlanning
êê '
=
êê( ))
_machinesPlanningRepository
ëë +
.
ëë+ ,
Find
ëë, 0
(
ëë0 1
o
ëë1 2
=>
ëë3 5
o
ëë6 7
.
ëë7 8
	MachineId
ëë8 A
.
ëëA B
Equals
ëëB H
(
ëëH I
command
ëëI P
.
ëëP Q
	MachineId
ëëQ Z
)
ëëZ [
&&
ëë\ ^
o
ëë_ `
.
ëë` a
Blok
ëëa e
.
ëëe f
Equals
ëëf l
(
ëël m
command
ëëm t
.
ëët u
Blok
ëëu y
)
ëëy z
)
ëëz {
.
íí+ ,
FirstOrDefault
íí, :
(
íí: ;
)
íí; <
;
íí< =
if
îî 
(
îî %
existingMachinePlanning
îî '
!=
îî( *
null
îî+ /
)
îî/ 0
{
ïï 
throw
ññ 
	Validator
ññ 
.
ññ  
ErrorValidation
ññ  /
(
ññ/ 0
(
ññ0 1
$str
ññ1 <
,
ññ< =
$str
ññ> k
)
ññk l
)
ññl m
;
ññm n
}
óó 
var
ôô 
machinePlanning
ôô 
=
ôô  !
await
ôô" '
Mediator
ôô( 0
.
ôô0 1
Send
ôô1 5
(
ôô5 6
command
ôô6 =
)
ôô= >
;
ôô> ?
return
õõ 
Ok
õõ 
(
õõ 
machinePlanning
õõ %
.
õõ% &
Identity
õõ& .
)
õõ. /
;
õõ/ 0
}
úú 	
[
ûû 	
HttpPut
ûû	 
(
ûû 
$str
ûû 
)
ûû 
]
ûû 
public
üü 
async
üü 
Task
üü 
<
üü 
IActionResult
üü '
>
üü' (
Put
üü) ,
(
üü, -
string
üü- 3
Id
üü4 6
,
üü6 7
[
††- .
FromBody
††. 6
]
††6 7)
UpdateEnginePlanningCommand
††7 R
command
††S Z
)
††Z [
{
°° 	
if
¢¢ 
(
¢¢ 
!
¢¢ 
Guid
¢¢ 
.
¢¢ 
TryParse
¢¢ 
(
¢¢ 
Id
¢¢ !
,
¢¢! "
out
¢¢# &
Guid
¢¢' +
Identity
¢¢, 4
)
¢¢4 5
)
¢¢5 6
{
££ 
return
§§ 
NotFound
§§ 
(
§§  
)
§§  !
;
§§! "
}
•• 
var
ßß %
existingMachinePlanning
ßß '
=
ßß( ))
_machinesPlanningRepository
®® +
.
®®+ ,
Find
®®, 0
(
®®0 1
o
®®1 2
=>
®®3 5
o
®®6 7
.
®®7 8
	MachineId
®®8 A
==
®®B D
command
®®E L
.
®®L M
	MachineId
®®M V
&&
®®W Y
o
®®Z [
.
®®[ \
Blok
®®\ `
==
®®a c
command
®®d k
.
®®k l
Blok
®®l p
)
®®p q
.
©©+ ,
FirstOrDefault
©©, :
(
©©: ;
)
©©; <
;
©©< =
if
´´ 
(
´´ %
existingMachinePlanning
´´ '
!=
´´( *
null
´´+ /
)
´´/ 0
{
¨¨ 
throw
≠≠ 
	Validator
≠≠ 
.
≠≠  
ErrorValidation
≠≠  /
(
≠≠/ 0
(
≠≠0 1
$str
≠≠1 <
,
≠≠< =
$str
≠≠> k
)
≠≠k l
)
≠≠l m
;
≠≠m n
}
ÆÆ 
command
∞∞ 
.
∞∞ 
SetId
∞∞ 
(
∞∞ 
Identity
∞∞ "
)
∞∞" #
;
∞∞# $
var
±± 
machinePlanning
±± 
=
±±  !
await
±±" '
Mediator
±±( 0
.
±±0 1
Send
±±1 5
(
±±5 6
command
±±6 =
)
±±= >
;
±±> ?
return
≥≥ 
Ok
≥≥ 
(
≥≥ 
machinePlanning
≥≥ %
.
≥≥% &
Identity
≥≥& .
)
≥≥. /
;
≥≥/ 0
}
¥¥ 	
[
∂∂ 	

HttpDelete
∂∂	 
(
∂∂ 
$str
∂∂ 
)
∂∂ 
]
∂∂ 
public
∑∑ 
async
∑∑ 
Task
∑∑ 
<
∑∑ 
IActionResult
∑∑ '
>
∑∑' (
Delete
∑∑) /
(
∑∑/ 0
string
∑∑0 6
Id
∑∑7 9
)
∑∑9 :
{
∏∏ 	
if
ππ 
(
ππ 
!
ππ 
Guid
ππ 
.
ππ 
TryParse
ππ 
(
ππ 
Id
ππ !
,
ππ! "
out
ππ# &
Guid
ππ' +
Identity
ππ, 4
)
ππ4 5
)
ππ5 6
{
∫∫ 
return
ªª 
NotFound
ªª 
(
ªª  
)
ªª  !
;
ªª! "
}
ºº 
var
ææ 
command
ææ 
=
ææ 
new
ææ )
RemoveEnginePlanningCommand
ææ 9
(
ææ9 :
)
ææ: ;
;
ææ; <
command
øø 
.
øø 
SetId
øø 
(
øø 
Identity
øø "
)
øø" #
;
øø# $
var
¡¡ 
machinePlanning
¡¡ 
=
¡¡  !
await
¡¡" '
Mediator
¡¡( 0
.
¡¡0 1
Send
¡¡1 5
(
¡¡5 6
command
¡¡6 =
)
¡¡= >
;
¡¡> ?
return
√√ 
Ok
√√ 
(
√√ 
machinePlanning
√√ %
.
√√% &
Identity
√√& .
)
√√. /
;
√√/ 0
}
ƒƒ 	
}
≈≈ 
}∆∆ ¨\
sD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Controllers\Api\MachineTypeDocumentController.cs
	namespace 	
Manufactures
 
. 
Controllers "
." #
Api# &
{ 
[ 
Produces 
( 
$str  
)  !
]! "
[ 
Route 

(
 
$str "
)" #
]# $
[ 
ApiController 
] 
[ 
	Authorize 
] 
public 

class )
MachineTypeDocumentController .
:/ 0
ControllerApiBase1 B
{ 
private 
readonly "
IMachineTypeRepository /"
_machineTypeRepository0 F
;F G
public )
MachineTypeDocumentController ,
(, -
IServiceProvider- =
serviceProvider> M
)M N
: 
base 
( 
serviceProvider "
)" #
{ 	"
_machineTypeRepository "
=# $
this 
. 
Storage 
. 
GetRepository *
<* +"
IMachineTypeRepository+ A
>A B
(B C
)C D
;D E
} 	
[ 	
HttpGet	 
] 
public 
async 
Task 
< 
IActionResult '
>' (
Get) ,
(, -
int- 0
page1 5
=6 7
$num8 9
,9 :
int  - 0
size  1 5
=  6 7
$num  8 :
,  : ;
string!!- 3
order!!4 9
=!!: ;
$str!!< @
,!!@ A
string""- 3
keyword""4 ;
=""< =
null""> B
,""B C
string##- 3
filter##4 :
=##; <
$str##= A
)##A B
{$$ 	
page%% 
=%% 
page%% 
-%% 
$num%% 
;%% 
var&& 
query&& 
=&& "
_machineTypeRepository'' &
.''& '
Query''' ,
.'', -
OrderByDescending''- >
(''> ?
item''? C
=>''D F
item''G K
.''K L
CreatedDate''L W
)''W X
;''X Y
var(( 
machineType(( 
=(( "
_machineTypeRepository)) &
.))& '
Find))' +
())+ ,
query)), 1
)))1 2
.**' (
Select**( .
(**. /
item**/ 3
=>**4 6
new**7 :"
MachineTypeDocumentDto**; Q
(**Q R
item**R V
)**V W
)**W X
;**X Y
if,, 
(,, 
!,, 
string,, 
.,, 
IsNullOrEmpty,, %
(,,% &
keyword,,& -
),,- .
),,. /
{-- 
machineType.. 
=.. 
machineType// 
.//  
Where//  %
(//% &
entity//& ,
=>//- /
entity//0 6
.//6 7
TypeName//7 ?
.//? @
Contains//@ H
(//H I
keyword//I P
,//P Q
StringComparison00E U
.00U V
OrdinalIgnoreCase00V g
)00g h
)00h i
;00i j
}11 
if33 
(33 
!33 
order33 
.33 
Contains33 
(33  
$str33  $
)33$ %
)33% &
{44 

Dictionary55 
<55 
string55 !
,55! "
string55# )
>55) *
orderDictionary55+ :
=55; <
JsonConvert66 
.66  
DeserializeObject66  1
<661 2

Dictionary662 <
<66< =
string66= C
,66C D
string66E K
>66K L
>66L M
(66M N
order66N S
)66S T
;66T U
var77 
key77 
=77 
orderDictionary77 )
.77) *
Keys77* .
.77. /
First77/ 4
(774 5
)775 6
.776 7
	Substring777 @
(77@ A
$num77A B
,77B C
$num77D E
)77E F
.77F G
ToUpper77G N
(77N O
)77O P
+77Q R
orderDictionary88 )
.88) *
Keys88* .
.88. /
First88/ 4
(884 5
)885 6
.886 7
	Substring887 @
(88@ A
$num88A B
)88B C
;88C D
System99 
.99 

Reflection99 !
.99! "
PropertyInfo99" .
prop99/ 3
=994 5
typeof996 <
(99< ="
MachineTypeDocumentDto99= S
)99S T
.99T U
GetProperty99U `
(99` a
key99a d
)99d e
;99e f
if;; 
(;; 
orderDictionary;; #
.;;# $
Values;;$ *
.;;* +
Contains;;+ 3
(;;3 4
$str;;4 9
);;9 :
);;: ;
{<< 
machineType== 
===  !
machineType==" -
.==- .
OrderBy==. 5
(==5 6
x==6 7
=>==8 :
prop==; ?
.==? @
GetValue==@ H
(==H I
x==I J
,==J K
null==L P
)==P Q
)==Q R
;==R S
}>> 
else?? 
{@@ 
machineTypeAA 
=AA  !
machineTypeAA" -
.AA- .
OrderByDescendingAA. ?
(AA? @
xAA@ A
=>AAB D
propAAE I
.AAI J
GetValueAAJ R
(AAR S
xAAS T
,AAT U
nullAAV Z
)AAZ [
)AA[ \
;AA\ ]
}BB 
}CC 
machineTypeEE 
=EE 
machineTypeEE %
.EE% &
SkipEE& *
(EE* +
pageEE+ /
*EE0 1
sizeEE2 6
)EE6 7
.EE7 8
TakeEE8 <
(EE< =
sizeEE= A
)EEA B
;EEB C
intFF 
	totalRowsFF 
=FF 
machineTypeFF '
.FF' (
CountFF( -
(FF- .
)FF. /
;FF/ 0
pageGG 
=GG 
pageGG 
+GG 
$numGG 
;GG 
awaitII 
TaskII 
.II 
YieldII 
(II 
)II 
;II 
returnKK 
OkKK 
(KK 
machineTypeKK !
,KK! "
infoKK# '
:KK' (
newKK) ,
{LL 
pageMM 
,MM 
sizeNN 
,NN 
totalOO 
=OO 
	totalRowsOO !
}PP 
)PP 
;PP 
}QQ 	
[SS 	
HttpGetSS	 
(SS 
$strSS 
)SS 
]SS 
publicTT 
asyncTT 
TaskTT 
<TT 
IActionResultTT '
>TT' (
GetTT) ,
(TT, -
stringTT- 3
IdTT4 6
)TT6 7
{UU 	
varVV 
IdentityVV 
=VV 
GuidVV 
.VV  
ParseVV  %
(VV% &
IdVV& (
)VV( )
;VV) *
varWW 
MachineTypeWW 
=WW "
_machineTypeRepositoryXX &
.XX& '
FindXX' +
(XX+ ,
itemXX, 0
=>XX1 3
itemXX4 8
.XX8 9
IdentityXX9 A
==XXB D
IdentityXXE M
)XXM N
.YY" #
FirstOrDefaultYY# 1
(YY1 2
)YY2 3
;YY3 4
await[[ 
Task[[ 
.[[ 
Yield[[ 
([[ 
)[[ 
;[[ 
if]] 
(]] 
MachineType]] 
==]] 
null]] #
)]]# $
{^^ 
return__ 
NotFound__ 
(__  
)__  !
;__! "
}`` 
elseaa 
{bb 
varcc 
resultcc 
=cc 
newcc  "
MachineTypeDocumentDtocc! 7
(cc7 8
MachineTypecc8 C
)ccC D
;ccD E
returndd 
Okdd 
(dd 
resultdd  
)dd  !
;dd! "
}ee 
}ff 	
[hh 	
HttpPosthh	 
]hh 
publicii 
asyncii 
Taskii 
<ii 
IActionResultii '
>ii' (
Postii) -
(ii- .
[ii. /
FromBodyii/ 7
]ii7 8$
AddNewMachineTypeCommandii8 P
commandiiQ X
)iiX Y
{jj 	
varkk 
machineTypekk 
=kk 
awaitkk #
Mediatorkk$ ,
.kk, -
Sendkk- 1
(kk1 2
commandkk2 9
)kk9 :
;kk: ;
returnmm 
Okmm 
(mm 
machineTypemm !
.mm! "
Identitymm" *
)mm* +
;mm+ ,
}nn 	
[pp 	
HttpPutpp	 
(pp 
$strpp 
)pp 
]pp 
publicqq 
asyncqq 
Taskqq 
<qq 
IActionResultqq '
>qq' (
Putqq) ,
(qq, -
stringqq- 3
Idqq4 6
,qq6 7
[rr- .
FromBodyrr. 6
]rr6 7,
 UpdateExistingMachineTypeCommandrr7 W
commandrrX _
)rr_ `
{ss 	
iftt 
(tt 
!tt 
Guidtt 
.tt 
TryParsett 
(tt 
Idtt !
,tt! "
outtt# &
Guidtt' +
Identitytt, 4
)tt4 5
)tt5 6
{uu 
returnvv 
NotFoundvv 
(vv  
)vv  !
;vv! "
}ww 
commandyy 
.yy 
SetIdyy 
(yy 
Identityyy "
)yy" #
;yy# $
varzz 
machineTypezz 
=zz 
awaitzz #
Mediatorzz$ ,
.zz, -
Sendzz- 1
(zz1 2
commandzz2 9
)zz9 :
;zz: ;
return|| 
Ok|| 
(|| 
machineType|| !
.||! "
Identity||" *
)||* +
;||+ ,
}}} 	
[ 	

HttpDelete	 
( 
$str 
) 
] 
public
ÄÄ 
async
ÄÄ 
Task
ÄÄ 
<
ÄÄ 
IActionResult
ÄÄ '
>
ÄÄ' (
Delete
ÄÄ) /
(
ÄÄ/ 0
string
ÄÄ0 6
Id
ÄÄ7 9
)
ÄÄ9 :
{
ÅÅ 	
if
ÇÇ 
(
ÇÇ 
!
ÇÇ 
Guid
ÇÇ 
.
ÇÇ 
TryParse
ÇÇ 
(
ÇÇ 
Id
ÇÇ !
,
ÇÇ! "
out
ÇÇ# &
Guid
ÇÇ' +
Identity
ÇÇ, 4
)
ÇÇ4 5
)
ÇÇ5 6
{
ÉÉ 
return
ÑÑ 
NotFound
ÑÑ 
(
ÑÑ  
)
ÑÑ  !
;
ÑÑ! "
}
ÖÖ 
var
áá 
command
áá 
=
áá 
new
áá .
 RemoveExistingMachineTypeCommand
áá >
(
áá> ?
)
áá? @
;
áá@ A
command
àà 
.
àà 
SetId
àà 
(
àà 
Identity
àà "
)
àà" #
;
àà# $
var
ää 
MachineType
ää 
=
ää 
await
ää #
Mediator
ää$ ,
.
ää, -
Send
ää- 1
(
ää1 2
command
ää2 9
)
ää9 :
;
ää: ;
return
åå 
Ok
åå 
(
åå 
MachineType
åå !
.
åå! "
Identity
åå" *
)
åå* +
;
åå+ ,
}
çç 	
}
éé 
}èè –=
pD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Controllers\Api\ManufactureOrderController.cs
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
Controllers

 "
{ 
[ 
Route 

(
 
$str 
)  
]  !
[ 
ApiController 
] 
public 

class &
ManufactureOrderController +
:, -
Barebone. 6
.6 7
Controllers7 B
.B C
ControllerApiBaseC T
{ 
private 
readonly '
IManufactureOrderRepository 4!
_manufactureOrderRepo5 J
;J K
public &
ManufactureOrderController )
() *
IServiceProvider* :
serviceProvider; J
)J K
:L M
baseN R
(R S
serviceProviderS b
)b c
{ 	!
_manufactureOrderRepo !
=" #
this$ (
.( )
Storage) 0
.0 1
GetRepository1 >
<> ?'
IManufactureOrderRepository? Z
>Z [
([ \
)\ ]
;] ^
} 	
[ 	
HttpGet	 
] 
public 
async 
Task 
< 
IActionResult '
>' (
Get) ,
(, -
)- .
{ 	
int 
page 
= 
$num 
, 
pageSize "
=# $
$num% '
;' (
int 
	totalRows 
= !
_manufactureOrderRepo 1
.1 2
Query2 7
.7 8
Count8 =
(= >
)> ?
;? @
var 
query 
= !
_manufactureOrderRepo -
.- .
Query. 3
.3 4
OrderByDescending4 E
(E F
oF G
=>H J
oK L
.L M
CreatedDateM X
)X Y
.Y Z
TakeZ ^
(^ _
pageSize_ g
)g h
.h i
Skipi m
(m n
pagen r
*s t
pageSizeu }
)} ~
;~ 
var 
	ordersDto 
= !
_manufactureOrderRepo 1
.1 2
Find2 6
(6 7
query7 <
)< =
.= >
Select> D
(D E
oE F
=>G I
newJ M
ManufactureOrderDtoN a
(a b
ob c
)c d
)d e
.e f
ToArrayf m
(m n
)n o
;o p
await!! 
Task!! 
.!! 
Yield!! 
(!! 
)!! 
;!! 
return## 
Ok## 
(## 
	ordersDto## 
,##  
info##! %
:##% &
new##' *
{$$ 
page%% 
,%% 
pageSize&& 
,&& 
count'' 
='' 
	totalRows'' !
}(( 
)(( 
;(( 
})) 	
[++ 	
HttpGet++	 
(++ 
$str++ 
)++ 
]++ 
public,, 
async,, 
Task,, 
<,, 
IActionResult,, '
>,,' (
Get,,) ,
(,,, -
string,,- 3
id,,4 6
),,6 7
{-- 	
var.. 
orderId.. 
=.. 
Guid.. 
... 
Parse.. $
(..$ %
id..% '
)..' (
;..( )
var00 
orderDto00 
=00 !
_manufactureOrderRepo00 0
.000 1
Find001 5
(005 6
o006 7
=>008 :
o00; <
.00< =
Identity00= E
==00F H
orderId00I P
)00P Q
.00Q R
Select00R X
(00X Y
o00Y Z
=>00[ ]
new00^ a
ManufactureOrderDto00b u
(00u v
o00v w
)00w x
)00x y
.00y z
FirstOrDefault	00z à
(
00à â
)
00â ä
;
00ä ã
await22 
Task22 
.22 
Yield22 
(22 
)22 
;22 
if44 
(44 
orderDto44 
==44 
null44  
)44  !
return55 
NotFound55 
(55  
)55  !
;55! "
else66 
return77 
Ok77 
(77 
orderDto77 "
)77" #
;77# $
}88 	
[:: 	
HttpPost::	 
]:: 
public;; 
async;; 
Task;; 
<;; 
IActionResult;; '
>;;' (
Post;;) -
(;;- .
[;;. /
FromBody;;/ 7
];;7 8 
PlaceOrderCommandOld;;8 L
command;;M T
);;T U
{<< 	
var== 
order== 
=== 
await== 
Mediator== &
.==& '
Send==' +
(==+ ,
command==, 3
)==3 4
;==4 5
return?? 
Ok?? 
(?? 
order?? 
.?? 
Identity?? $
)??$ %
;??% &
}@@ 	
[BB 	
HttpPutBB	 
(BB 
$strBB 
)BB 
]BB 
publicCC 
asyncCC 
TaskCC 
<CC 
IActionResultCC '
>CC' (
PutCC) ,
(CC, -
stringCC- 3
idCC4 6
,CC6 7
[CC8 9
FromBodyCC9 A
]CCA B!
UpdateOrderCommandOldCCB W
commandCCX _
)CC_ `
{DD 	
ifEE 
(EE 
!EE 
GuidEE 
.EE 
TryParseEE 
(EE 
idEE !
,EE! "
outEE# &
GuidEE' +
orderIdEE, 3
)EE3 4
)EE4 5
returnFF 
NotFoundFF 
(FF  
)FF  !
;FF! "
commandHH 
.HH 
SetIdHH 
(HH 
orderIdHH !
)HH! "
;HH" #
varJJ 
orderJJ 
=JJ 
awaitJJ 
MediatorJJ &
.JJ& '
SendJJ' +
(JJ+ ,
commandJJ, 3
)JJ3 4
;JJ4 5
returnLL 
OkLL 
(LL 
orderLL 
.LL 
IdentityLL $
)LL$ %
;LL% &
}MM 	
[OO 	

HttpDeleteOO	 
(OO 
$strOO 
)OO 
]OO 
publicPP 
asyncPP 
TaskPP 
<PP 
IActionResultPP '
>PP' (
DeletePP) /
(PP/ 0
stringPP0 6
idPP7 9
)PP9 :
{QQ 	
ifRR 
(RR 
!RR 
GuidRR 
.RR 
TryParseRR 
(RR 
idRR !
,RR! "
outRR# &
GuidRR' +
orderIdRR, 3
)RR3 4
)RR4 5
returnSS 
NotFoundSS 
(SS  
)SS  !
;SS! "
varUU 
commandUU 
=UU 
newUU !
RemoveOrderCommandOldUU 3
(UU3 4
)UU4 5
;UU5 6
commandVV 
.VV 
SetIdVV 
(VV 
orderIdVV !
)VV! "
;VV" #
varXX 
orderXX 
=XX 
awaitXX 
MediatorXX &
.XX& '
SendXX' +
(XX+ ,
commandXX, 3
)XX3 4
;XX4 5
returnZZ 
OkZZ 
(ZZ 
orderZZ 
.ZZ 
IdentityZZ $
)ZZ$ %
;ZZ% &
}[[ 	
}\\ 
}]] ã`
lD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Controllers\Api\MaterialTypeController.cs
	namespace 	
Manufactures
 
. 
Controllers "
." #
Api# &
{ 
[ 
Produces 
( 
$str  
)  !
]! "
[ 
Route 

(
 
$str #
)# $
]$ %
[ 
ApiController 
] 
[ 
	Authorize 
] 
public 

class "
MaterialTypeController '
:( )
ControllerApiBase* ;
{ 
private 
readonly #
IMaterialTypeRepository 0#
_materialTypeRepository1 H
;H I
public "
MaterialTypeController %
(% &
IServiceProvider& 6
serviceProvider7 F
,F G
IWorkContext& 2
workContext3 >
)> ?
:@ A
baseB F
(F G
serviceProviderG V
)V W
{ 	#
_materialTypeRepository #
=$ %
this 
. 
Storage 
. 
GetRepository *
<* +#
IMaterialTypeRepository+ B
>B C
(C D
)D E
;E F
} 	
[ 	
HttpGet	 
] 
public   
async   
Task   
<   
IActionResult   '
>  ' (
Get  ) ,
(  , -
int  - 0
page  1 5
=  6 7
$num  8 9
,  9 :
int!!- 0
size!!1 5
=!!6 7
$num!!8 :
,!!: ;
string""- 3
order""4 9
="": ;
$str""< @
,""@ A
string##- 3
keyword##4 ;
=##< =
null##> B
,##B C
string$$- 3
filter$$4 :
=$$; <
$str$$= A
)$$A B
{%% 	
page&& 
=&& 
page&& 
-&& 
$num&& 
;&& 
var'' 
query'' 
='' #
_materialTypeRepository(( '
.((' (
Query((( -
.((- .
OrderByDescending((. ?
(((? @
item((@ D
=>((E G
item((H L
.((L M
CreatedDate((M X
)((X Y
;((Y Z
var)) !
materialTypeDocuments)) %
=))& '#
_materialTypeRepository** '
.**' (
Find**( ,
(**, -
query**- 2
)**2 3
.**3 4
Select**4 :
(**: ;
item**; ?
=>**@ B
new**C F
MaterialTypeListDto**G Z
(**Z [
item**[ _
)**_ `
)**` a
;**a b
if,, 
(,, 
!,, 
string,, 
.,, 
IsNullOrEmpty,, %
(,,% &
keyword,,& -
),,- .
),,. /
{-- !
materialTypeDocuments.. $
=..% &!
materialTypeDocuments// )
.00 
Where00 
(00 
entity00 %
=>00& (
entity00) /
.00/ 0
Code000 4
.004 5
Contains005 =
(00= >
keyword00> E
,00E F
StringComparison11> N
.11N O$
CurrentCultureIgnoreCase11O g
)11g h
||11i k
entity22) /
.22/ 0
Name220 4
.224 5
Contains225 =
(22= >
keyword22> E
,22E F
StringComparison33> N
.33N O$
CurrentCultureIgnoreCase33O g
)33g h
)33h i
;33i j
}44 
if66 
(66 
!66 
order66 
.66 
Contains66 
(66  
$str66  $
)66$ %
)66% &
{77 

Dictionary88 
<88 
string88 !
,88! "
string88# )
>88) *
orderDictionary88+ :
=88; <
JsonConvert99 
.99  
DeserializeObject99  1
<991 2

Dictionary992 <
<99< =
string99= C
,99C D
string99E K
>99K L
>99L M
(99M N
order99N S
)99S T
;99T U
var:: 
key:: 
=:: 
orderDictionary:: )
.::) *
Keys::* .
.::. /
First::/ 4
(::4 5
)::5 6
.::6 7
	Substring::7 @
(::@ A
$num::A B
,::B C
$num::D E
)::E F
.::F G
ToUpper::G N
(::N O
)::O P
+::Q R
orderDictionary;; )
.;;) *
Keys;;* .
.;;. /
First;;/ 4
(;;4 5
);;5 6
.;;6 7
	Substring;;7 @
(;;@ A
$num;;A B
);;B C
;;;C D
System<< 
.<< 

Reflection<< !
.<<! "
PropertyInfo<<" .
prop<</ 3
=<<4 5
typeof<<6 <
(<<< =
MaterialTypeListDto<<= P
)<<P Q
.<<Q R
GetProperty<<R ]
(<<] ^
key<<^ a
)<<a b
;<<b c
if>> 
(>> 
orderDictionary>> #
.>># $
Values>>$ *
.>>* +
Contains>>+ 3
(>>3 4
$str>>4 9
)>>9 :
)>>: ;
{?? !
materialTypeDocuments@@ )
=@@* +!
materialTypeDocumentsAA -
.AA- .
OrderByAA. 5
(AA5 6
xAA6 7
=>AA8 :
propAA; ?
.AA? @
GetValueAA@ H
(AAH I
xAAI J
,AAJ K
nullAAL P
)AAP Q
)AAQ R
;AAR S
}BB 
elseCC 
{DD !
materialTypeDocumentsEE )
=EE* +!
materialTypeDocumentsFF -
.FF- .
OrderByDescendingFF. ?
(FF? @
xFF@ A
=>FFB D
propFFE I
.FFI J
GetValueFFJ R
(FFR S
xFFS T
,FFT U
nullFFV Z
)FFZ [
)FF[ \
;FF\ ]
}GG 
}HH !
materialTypeDocumentsJJ !
=JJ" #!
materialTypeDocumentsJJ$ 9
.JJ9 :
SkipJJ: >
(JJ> ?
pageJJ? C
*JJD E
sizeJJF J
)JJJ K
.JJK L
TakeJJL P
(JJP Q
sizeJJQ U
)JJU V
;JJV W
intKK 
	totalRowsKK 
=KK !
materialTypeDocumentsKK 1
.KK1 2
CountKK2 7
(KK7 8
)KK8 9
;KK9 :
pageLL 
=LL 
pageLL 
+LL 
$numLL 
;LL 
awaitNN 
TaskNN 
.NN 
YieldNN 
(NN 
)NN 
;NN 
returnPP 
OkPP 
(PP !
materialTypeDocumentsPP +
,PP+ ,
infoPP- 1
:PP1 2
newPP3 6
{QQ 
pageRR 
,RR 
sizeSS 
,SS 
totalTT 
=TT 
	totalRowsTT !
}UU 
)UU 
;UU 
}VV 	
[XX 	
HttpGetXX	 
(XX 
$strXX 
)XX 
]XX 
publicYY 
asyncYY 
TaskYY 
<YY 
IActionResultYY '
>YY' (
GetYY) ,
(YY, -
stringYY- 3
IdYY4 6
)YY6 7
{ZZ 	
var[[ 
Identity[[ 
=[[ 
Guid[[ 
.[[  
Parse[[  %
([[% &
Id[[& (
)[[( )
;[[) *
var\\ 
materialTypeDto\\ 
=\\  !#
_materialTypeRepository]] '
.]]' (
Find]]( ,
(]], -
item]]- 1
=>]]2 4
item]]5 9
.]]9 :
Identity]]: B
==]]C E
Identity]]F N
)]]N O
.^^' (
Select^^( .
(^^. /
item^^/ 3
=>^^4 6
new^^7 :#
MaterialTypeDocumentDto^^; R
(^^R S
item^^S W
)^^W X
)^^X Y
.__' (
FirstOrDefault__( 6
(__6 7
)__7 8
;__8 9
await`` 
Task`` 
.`` 
Yield`` 
(`` 
)`` 
;`` 
ifbb 
(bb 
materialTypeDtobb 
==bb  "
nullbb# '
)bb' (
{cc 
returndd 
NotFounddd 
(dd  
)dd  !
;dd! "
}ee 
elseff 
{gg 
returnhh 
Okhh 
(hh 
materialTypeDtohh )
)hh) *
;hh* +
}ii 
}jj 	
[ll 	
HttpPostll	 
]ll 
publicmm 
asyncmm 
Taskmm 
<mm 
IActionResultmm '
>mm' (
Postmm) -
(mm- .
[mm. /
FromBodymm/ 7
]mm7 8$
PlaceMaterialTypeCommandmm8 P
commandmmQ X
)mmX Y
{nn 	
varoo 
materialTypeoo 
=oo 
awaitoo $
Mediatoroo% -
.oo- .
Sendoo. 2
(oo2 3
commandoo3 :
)oo: ;
;oo; <
returnqq 
Okqq 
(qq 
materialTypeqq "
.qq" #
Identityqq# +
)qq+ ,
;qq, -
}rr 	
[tt 	
HttpPuttt	 
(tt 
$strtt 
)tt 
]tt 
publicuu 
asyncuu 
Taskuu 
<uu 
IActionResultuu '
>uu' (
Putuu) ,
(uu, -
stringuu- 3
Iduu4 6
,uu6 7
[vv- .
FromBodyvv. 6
]vv6 7%
UpdateMaterialTypeCommandvv7 P
commandvvQ X
)vvX Y
{ww 	
ifxx 
(xx 
!xx 
Guidxx 
.xx 
TryParsexx 
(xx 
Idxx !
,xx! "
outxx# &
Guidxx' +
Identityxx, 4
)xx4 5
)xx5 6
{yy 
returnzz 
NotFoundzz 
(zz  
)zz  !
;zz! "
}{{ 
command}} 
.}} 
SetId}} 
(}} 
Identity}} "
)}}" #
;}}# $
var~~ 
materialType~~ 
=~~ 
await~~ $
Mediator~~% -
.~~- .
Send~~. 2
(~~2 3
command~~3 :
)~~: ;
;~~; <
return
ÄÄ 
Ok
ÄÄ 
(
ÄÄ 
materialType
ÄÄ "
.
ÄÄ" #
Identity
ÄÄ# +
)
ÄÄ+ ,
;
ÄÄ, -
}
ÅÅ 	
[
ÉÉ 	

HttpDelete
ÉÉ	 
(
ÉÉ 
$str
ÉÉ 
)
ÉÉ 
]
ÉÉ 
public
ÑÑ 
async
ÑÑ 
Task
ÑÑ 
<
ÑÑ 
IActionResult
ÑÑ '
>
ÑÑ' (
Delete
ÑÑ) /
(
ÑÑ/ 0
string
ÑÑ0 6
Id
ÑÑ7 9
)
ÑÑ9 :
{
ÖÖ 	
if
ÜÜ 
(
ÜÜ 
!
ÜÜ 
Guid
ÜÜ 
.
ÜÜ 
TryParse
ÜÜ 
(
ÜÜ 
Id
ÜÜ !
,
ÜÜ! "
out
ÜÜ# &
Guid
ÜÜ' +
Identity
ÜÜ, 4
)
ÜÜ4 5
)
ÜÜ5 6
{
áá 
return
àà 
NotFound
àà 
(
àà  
)
àà  !
;
àà! "
}
ââ 
var
ãã 
command
ãã 
=
ãã 
new
ãã '
RemoveMaterialTypeCommand
ãã 7
(
ãã7 8
)
ãã8 9
;
ãã9 :
command
åå 
.
åå 
SetId
åå 
(
åå 
Identity
åå "
)
åå" #
;
åå# $
var
éé 
materialType
éé 
=
éé 
await
éé $
Mediator
éé% -
.
éé- .
Send
éé. 2
(
éé2 3
command
éé3 :
)
éé: ;
;
éé; <
return
êê 
Ok
êê 
(
êê 
materialType
êê "
.
êê" #
Identity
êê# +
)
êê+ ,
;
êê, -
}
ëë 	
}
íí 
}ìì Ìk
hD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Controllers\Api\OperatorController.cs
	namespace 	
Manufactures
 
. 
Controllers "
." #
Api# &
{ 
[ 
Produces 
( 
$str  
)  !
]! "
[ 
Route 

(
 
$str 
) 
]  
[ 
ApiController 
] 
[ 
	Authorize 
] 
public 

class 
OperatorController #
:$ %
ControllerApiBase& 7
{ 
private 
readonly 
IOperatorRepository ,
_OperatorRepository- @
;@ A
public 
OperatorController !
(! "
IServiceProvider" 2
serviceProvider3 B
,B C
IWorkContext" .
workContext/ :
): ;
:< =
base> B
(B C
serviceProviderC R
)R S
{ 	
_OperatorRepository 
=  !
this 
. 
Storage 
. 
GetRepository *
<* +
IOperatorRepository+ >
>> ?
(? @
)@ A
;A B
}   	
["" 	
HttpGet""	 
]"" 
public## 
async## 
Task## 
<## 
IActionResult## '
>##' (
Get##) ,
(##, -
int##- 0
page##1 5
=##6 7
$num##8 9
,##9 :
int$$- 0
size$$1 5
=$$6 7
$num$$8 :
,$$: ;
string%%- 3
order%%4 9
=%%: ;
$str%%< @
,%%@ A
string&&- 3
keyword&&4 ;
=&&< =
null&&> B
,&&B C
string''- 3
filter''4 :
=''; <
$str''= A
)''A B
{(( 	
page)) 
=)) 
page)) 
-)) 
$num)) 
;)) 
var** 
query** 
=** 
_OperatorRepository++ #
.++# $
Query++$ )
.++) *
OrderByDescending++* ;
(++; <
item++< @
=>++A C
item++D H
.++H I
CreatedDate++I T
)++T U
;++U V
var,, 
operatorDocuments,, !
=,," #
_OperatorRepository-- #
.--# $
Find--$ (
(--( )
query--) .
)--. /
.--/ 0
Select--0 6
(--6 7
x--7 8
=>--9 ;
new--< ?
OperatorListDto--@ O
(--O P
x--P Q
)--Q R
)--R S
;--S T
if// 
(// 
!// 
string// 
.// 
IsNullOrEmpty// %
(//% &
keyword//& -
)//- .
)//. /
{00 
operatorDocuments11 !
=11" #
operatorDocuments22 %
.33 
Where33 
(33 
entity33 %
=>33& (
entity33) /
.33/ 0
Username330 8
.44/ 0
Contains440 8
(448 9
keyword449 @
,44@ A
StringComparison559 I
.55I J
OrdinalIgnoreCase55J [
)55[ \
)55\ ]
.66 
ToList66 
(66  
)66  !
;66! "
}77 
if99 
(99 
!99 
order99 
.99 
Contains99 
(99  
$str99  $
)99$ %
)99% &
{:: 

Dictionary;; 
<;; 
string;; !
,;;! "
string;;# )
>;;) *
orderDictionary;;+ :
=;;; <
JsonConvert<< 
.<<  
DeserializeObject<<  1
<<<1 2

Dictionary<<2 <
<<<< =
string<<= C
,<<C D
string<<E K
><<K L
><<L M
(<<M N
order<<N S
)<<S T
;<<T U
var== 
key== 
=== 
orderDictionary== )
.==) *
Keys==* .
.==. /
First==/ 4
(==4 5
)==5 6
.==6 7
	Substring==7 @
(==@ A
$num==A B
,==B C
$num==D E
)==E F
.==F G
ToUpper==G N
(==N O
)==O P
+==Q R
orderDictionary>> )
.>>) *
Keys>>* .
.>>. /
First>>/ 4
(>>4 5
)>>5 6
.>>6 7
	Substring>>7 @
(>>@ A
$num>>A B
)>>B C
;>>C D
System?? 
.?? 

Reflection?? !
.??! "
PropertyInfo??" .
prop??/ 3
=??4 5
typeof??6 <
(??< =
OperatorListDto??= L
)??L M
.??M N
GetProperty??N Y
(??Y Z
key??Z ]
)??] ^
;??^ _
ifAA 
(AA 
orderDictionaryAA #
.AA# $
ValuesAA$ *
.AA* +
ContainsAA+ 3
(AA3 4
$strAA4 9
)AA9 :
)AA: ;
{BB 
operatorDocumentsCC %
=CC& '
operatorDocumentsDD )
.DD) *
OrderByDD* 1
(DD1 2
xDD2 3
=>DD4 6
propDD7 ;
.DD; <
GetValueDD< D
(DDD E
xDDE F
,DDF G
nullDDH L
)DDL M
)DDM N
.DDN O
ToListDDO U
(DDU V
)DDV W
;DDW X
}EE 
elseFF 
{GG 
operatorDocumentsHH %
=HH& '
operatorDocumentsII )
.II) *
OrderByDescendingII* ;
(II; <
xII< =
=>II> @
propIIA E
.IIE F
GetValueIIF N
(IIN O
xIIO P
,IIP Q
nullIIR V
)IIV W
)IIW X
.IIX Y
ToListIIY _
(II_ `
)II` a
;IIa b
}JJ 
}KK 
operatorDocumentsMM 
=MM 
operatorDocumentsMM  1
.MM1 2
SkipMM2 6
(MM6 7
pageMM7 ;
*MM< =
sizeMM> B
)MMB C
.MMC D
TakeMMD H
(MMH I
sizeMMI M
)MMM N
.MMN O
ToListMMO U
(MMU V
)MMV W
;MMW X
intNN 
	totalRowsNN 
=NN 
operatorDocumentsNN -
.NN- .
CountNN. 3
(NN3 4
)NN4 5
;NN5 6
pageOO 
=OO 
pageOO 
+OO 
$numOO 
;OO 
varQQ 
resultDocumentsQQ 
=QQ  !
operatorDocumentsQQ" 3
;QQ3 4
awaitSS 
TaskSS 
.SS 
YieldSS 
(SS 
)SS 
;SS 
returnUU 
OkUU 
(UU 
resultDocumentsUU %
,UU% &
infoUU' +
:UU+ ,
newUU- 0
{VV 
pageWW 
,WW 
sizeXX 
,XX 
totalYY 
=YY 
	totalRowsYY !
}ZZ 
)ZZ 
;ZZ 
}[[ 	
[]] 	
HttpGet]]	 
(]] 
$str]] 
)]] 
]]] 
public^^ 
async^^ 
Task^^ 
<^^ 
IActionResult^^ '
>^^' (
Get^^) ,
(^^, -
string^^- 3
Id^^4 6
)^^6 7
{__ 	
var`` 
Identity`` 
=`` 
Guid`` 
.``  
Parse``  %
(``% &
Id``& (
)``( )
;``) *
varaa 
operatorDocumentaa  
=aa! "
_OperatorRepositorybb #
.bb# $
Findbb$ (
(bb( )
itembb) -
=>bb. 0
itembb1 5
.bb5 6
Identitybb6 >
==bb? A
IdentitybbB J
)bbJ K
.bbK L
FirstOrDefaultbbL Z
(bbZ [
)bb[ \
;bb\ ]
awaitcc 
Taskcc 
.cc 
Yieldcc 
(cc 
)cc 
;cc 
ifee 
(ee 
operatorDocumentee  
==ee! #
nullee$ (
)ee( )
{ff 
returngg 
NotFoundgg 
(gg  
)gg  !
;gg! "
}hh 
elseii 
{jj 
varkk 

resultDatakk 
=kk  
newkk! $
OperatorByIdDtokk% 4
(kk4 5
operatorDocumentkk5 E
)kkE F
;kkF G
returnmm 
Okmm 
(mm 

resultDatamm $
)mm$ %
;mm% &
}nn 
}oo 	
[qq 	
HttpPostqq	 
]qq 
publicrr 
asyncrr 
Taskrr 
<rr 
IActionResultrr '
>rr' (
Postrr) -
(rr- .
[rr. /
FromBodyrr/ 7
]rr7 8
AddOperatorCommandrr8 J
commandrrK R
)rrR S
{ss 	
vartt 
	mongodbIdtt 
=tt 
commandtt #
.tt# $
CoreAccounttt$ /
.tt/ 0
MongoIdtt0 7
;tt7 8
varvv 
existingOperatorvv  
=vv! "
_OperatorRepositoryww #
.xx 
Queryxx 
.yy 
Whereyy 
(yy 
oyy 
=>yy 
oyy  !
.yy! "
CoreAccountyy" -
.zz! "
Deserializezz" -
<zz- .
CoreAccountzz. 9
>zz9 :
(zz: ;
)zz; <
.{{! "
MongoId{{" )
.{{) *
Equals{{* 0
({{0 1
	mongodbId{{1 :
){{: ;
&&{{< >
o||  !
.||! "
Group||" '
.||' (
Equals||( .
(||. /
command||/ 6
.||6 7
Group||7 <
)||< =
)||= >
.}} 
FirstOrDefault}} #
(}}# $
)}}$ %
;}}% &
if 
( 
existingOperator 
!=  "
null# '
)' (
{
ÄÄ 
throw
ÅÅ 
	Validator
ÅÅ 
.
ÇÇ 
ErrorValidation
ÇÇ $
(
ÇÇ$ %
(
ÇÇ% &
$str
ÇÇ& *
,
ÇÇ* +
$str
ÉÉ& U
)
ÉÉU V
)
ÉÉV W
;
ÉÉW X
}
ÑÑ 
var
ÜÜ 
operatorDocument
ÜÜ  
=
ÜÜ! "
await
ÜÜ# (
Mediator
ÜÜ) 1
.
ÜÜ1 2
Send
ÜÜ2 6
(
ÜÜ6 7
command
ÜÜ7 >
)
ÜÜ> ?
;
ÜÜ? @
return
àà 
Ok
àà 
(
àà 
operatorDocument
àà &
.
àà& '
Identity
àà' /
)
àà/ 0
;
àà0 1
}
ââ 	
[
ãã 	
HttpPut
ãã	 
(
ãã 
$str
ãã 
)
ãã 
]
ãã 
public
åå 
async
åå 
Task
åå 
<
åå 
IActionResult
åå '
>
åå' (
Put
åå) ,
(
åå, -
string
åå- 3
Id
åå4 6
,
åå6 7
[
çç- .
FromBody
çç. 6
]
çç6 7#
UpdateOperatorCommand
çç7 L
command
ççM T
)
ççT U
{
éé 	
if
èè 
(
èè 
!
èè 
Guid
èè 
.
èè 
TryParse
èè 
(
èè 
Id
èè !
,
èè! "
out
èè# &
Guid
èè' +
Identity
èè, 4
)
èè4 5
)
èè5 6
{
êê 
return
ëë 
NotFound
ëë 
(
ëë  
)
ëë  !
;
ëë! "
}
íí 
command
îî 
.
îî 
SetId
îî 
(
îî 
Identity
îî "
)
îî" #
;
îî# $
var
ïï 
operatorDocument
ïï  
=
ïï! "
await
ïï# (
Mediator
ïï) 1
.
ïï1 2
Send
ïï2 6
(
ïï6 7
command
ïï7 >
)
ïï> ?
;
ïï? @
return
óó 
Ok
óó 
(
óó 
operatorDocument
óó &
.
óó& '
Identity
óó' /
)
óó/ 0
;
óó0 1
}
òò 	
[
öö 	

HttpDelete
öö	 
(
öö 
$str
öö 
)
öö 
]
öö 
public
õõ 
async
õõ 
Task
õõ 
<
õõ 
IActionResult
õõ '
>
õõ' (
Delete
õõ) /
(
õõ/ 0
string
õõ0 6
Id
õõ7 9
)
õõ9 :
{
úú 	
if
ùù 
(
ùù 
!
ùù 
Guid
ùù 
.
ùù 
TryParse
ùù 
(
ùù 
Id
ùù !
,
ùù! "
out
ùù# &
Guid
ùù' +
Identity
ùù, 4
)
ùù4 5
)
ùù5 6
{
ûû 
return
üü 
NotFound
üü 
(
üü  
)
üü  !
;
üü! "
}
†† 
var
¢¢ 
command
¢¢ 
=
¢¢ 
new
¢¢ #
RemoveOperatorCommand
¢¢ 3
(
¢¢3 4
)
¢¢4 5
;
¢¢5 6
command
££ 
.
££ 
SetId
££ 
(
££ 
Identity
££ "
)
££" #
;
££# $
var
•• 
operatorDocument
••  
=
••! "
await
••# (
Mediator
••) 1
.
••1 2
Send
••2 6
(
••6 7
command
••7 >
)
••> ?
;
••? @
return
ßß 
Ok
ßß 
(
ßß 
operatorDocument
ßß &
.
ßß& '
Identity
ßß' /
)
ßß/ 0
;
ßß0 1
}
®® 	
}
©© 
}™™ ≠
mD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Controllers\Api\OrderDocumentController.cs
	namespace 	
Manufactures
 
. 
Controllers "
." #
Api# &
{ 
[ 
Produces 
( 
$str  
)  !
]! "
[ 
Route 

(
 
$str 
) 
] 
[ 
ApiController 
] 
[ 
	Authorize 
] 
public 

class #
OrderDocumentController (
:) *
ControllerApiBase+ <
{ 
private   
readonly   +
IWeavingOrderDocumentRepository   8+
_weavingOrderDocumentRepository!!/ N
;!!N O
private"" 
readonly"" )
IFabricConstructionRepository"" 6+
_constructionDocumentRepository##/ N
;##N O
private$$ 
readonly$$ (
IEstimationProductRepository$$ 5(
_estimationProductRepository%%/ K
;%%K L
private&& 
readonly&& #
IYarnDocumentRepository&& 0#
_yarnDocumentRepository''/ F
;''F G
public)) #
OrderDocumentController)) &
())& '
IServiceProvider))' 7
serviceProvider))8 G
,))G H
IWorkContext**' 3
workContext**4 ?
)**? @
:**A B
base**C G
(**G H
serviceProvider**H W
)**W X
{++ 	+
_weavingOrderDocumentRepository,, +
=,,, -
this-- 
.-- 
Storage-- 
.-- 
GetRepository-- *
<--* ++
IWeavingOrderDocumentRepository--+ J
>--J K
(--K L
)--L M
;--M N+
_constructionDocumentRepository.. +
=.., -
this// 
.// 
Storage// 
.// 
GetRepository// *
<//* +)
IFabricConstructionRepository//+ H
>//H I
(//I J
)//J K
;//K L(
_estimationProductRepository00 (
=00) *
this11 
.11 
Storage11 
.11 
GetRepository11 *
<11* +(
IEstimationProductRepository11+ G
>11G H
(11H I
)11I J
;11J K#
_yarnDocumentRepository22 #
=22$ %
this33 
.33 
Storage33 
.33 
GetRepository33 *
<33* +#
IYarnDocumentRepository33+ B
>33B C
(33C D
)33D E
;33E F
}44 	
[66 	
HttpGet66	 
]66 
[77 	
Route77	 
(77 
$str77 %
)77% &
]77& '
public88 
async88 
Task88 
<88 
IActionResult88 '
>88' (
GetOrderNumber88) 7
(887 8
)888 9
{99 	
var:: 
orderNumber:: 
=:: 
await;; +
_weavingOrderDocumentRepository;; 5
.;;5 6!
GetWeavingOrderNumber;;6 K
(;;K L
);;L M
;;;M N
return== 
Ok== 
(== 
orderNumber== !
)==! "
;==" #
}>> 	
[@@ 	
HttpGet@@	 
(@@ 
$str@@ c
)@@c d
]@@d e
publicAA 
asyncAA 
TaskAA 
<AA 
IActionResultAA '
>AA' (
GetAA) ,
(AA, -
stringAA- 3
monthAA4 9
,AA9 :
stringBB- 3
yearBB4 8
,BB8 9
stringCC- 3
unitCC4 8
,CC8 9
intDD- 0
unitIdDD1 7
,DD7 8
stringEE- 3
statusEE4 :
)EE: ;
{FF 	
varGG 
acceptRequestGG 
=GG 
RequestGG  '
.GG' (
HeadersGG( /
.GG/ 0
ValuesGG0 6
.GG6 7
ToListGG7 =
(GG= >
)GG> ?
;GG? @
varHH 
indexHH 
=HH 
acceptRequestHH %
.HH% &
IndexOfHH& -
(HH- .
$strHH. ?
)HH? @
>HHA B
$numHHC D
;HHD E
varJJ 

resultDataJJ 
=JJ 
newJJ  
ListJJ! %
<JJ% &"
OrderReportBySearchDtoJJ& <
>JJ< =
(JJ= >
)JJ> ?
;JJ? @
varKK 
queryKK 
=KK +
_weavingOrderDocumentRepositoryLL /
.MM 
QueryMM 
.MM 
OrderByDescendingMM ,
(MM, -
itemMM- 1
=>MM2 4
itemMM5 9
.MM9 :
CreatedDateMM: E
)MME F
;MMF G
varNN 
orderDtoNN 
=NN +
_weavingOrderDocumentRepositoryOO /
.PP 
FindPP 
(PP 
queryPP 
)PP  
.PP  !
WherePP! &
(PP& '
entityPP' -
=>PP. 0
entityPP1 7
.PP7 8
PeriodPP8 >
.PP> ?
MonthPP? D
==PPE G
(PPH I
monthPPI N
)PPN O
&&PPP R
entityQQ1 7
.QQ7 8
PeriodQQ8 >
.QQ> ?
YearQQ? C
==QQD F
(QQG H
yearQQH L
)QQL M
&&QQN P
entityRR1 7
.RR7 8
UnitIdRR8 >
.RR> ?
ValueRR? D
==RRE G
(RRH I
unitIdRRI O
)RRO P
)RRP Q
;RRQ R
ifTT 
(TT 
statusTT 
.TT 
EqualsTT 
(TT 
	ConstantsTT '
.TT' (
ONORDERTT( /
)TT/ 0
)TT0 1
{UU 
orderDtoVV 
=VV 
orderDtoVV #
.VV# $
WhereVV$ )
(VV) *
eVV* +
=>VV, .
eVV/ 0
.VV0 1
OrderStatusVV1 <
==VV= ?
	ConstantsVV@ I
.VVI J
ONORDERVVJ Q
)VVQ R
.VVR S
ToListVVS Y
(VVY Z
)VVZ [
;VV[ \
}WW 
ifYY 
(YY 
statusYY 
.YY 
EqualsYY 
(YY 
	ConstantsYY '
.YY' (
ALLYY( +
)YY+ ,
)YY, -
{ZZ 
orderDto[[ 
=[[ 
orderDto[[ #
.[[# $
Where[[$ )
([[) *
e[[* +
=>[[, .
e[[/ 0
.[[0 1
OrderStatus[[1 <
!=[[= ?
$str[[@ B
)[[B C
.[[C D
ToList[[D J
([[J K
)[[K L
;[[L M
}\\ 
foreach^^ 
(^^ 
var^^ 
order^^ 
in^^ !
orderDto^^" *
)^^* +
{__ 
var``  
constructionDocument`` (
=``) *+
_constructionDocumentRepositoryaa 3
.bb 
Findbb 
(bb 
ebb 
=>bb  "
ebb# $
.bb$ %
Identitybb% -
.bb- .
Equalsbb. 4
(bb4 5
orderbb5 :
.bb: ;
ConstructionIdbb; I
.bbI J
ValuebbJ O
)bbO P
)bbP Q
.cc 
FirstOrDefaultcc '
(cc' (
)cc( )
;cc) *
varee 
estimationQueryee #
=ee$ %(
_estimationProductRepositoryff ,
.ff, -
Queryff- 2
.ff2 3
OrderByDescendingff3 D
(ffD E
itemffE I
=>ffJ L
itemffM Q
.ffQ R
CreatedDateffR ]
)ff] ^
;ff^ _
vargg 
estimationDocumentgg &
=gg' ((
_estimationProductRepositoryhh 0
.hh0 1
Findhh1 5
(hh5 6
estimationQueryhh6 E
.hhE F
IncludehhF M
(hhM N
phhN O
=>hhP R
phhS T
.hhT U
EstimationProductshhU g
)hhg h
)hhh i
.ii0 1
Whereii1 6
(ii6 7
vii7 8
=>ii9 ;
vii< =
.ii= >
Periodii> D
.iiD E
MonthiiE J
.iiJ K
EqualsiiK Q
(iiQ R
orderiiR W
.iiW X
PeriodiiX ^
.ii^ _
Monthii_ d
)iid e
&&iif h
viii j
.iij k
Periodiik q
.iiq r
Yeariir v
.iiv w
Equalsiiw }
(ii} ~
order	ii~ É
.
iiÉ Ñ
Period
iiÑ ä
.
iiä ã
Year
iiã è
)
iiè ê
&&
iië ì
v
iiî ï
.
iiï ñ
UnitId
iiñ ú
.
iiú ù
Value
iiù ¢
.
ii¢ £
Equals
ii£ ©
(
ii© ™
order
ii™ Ø
.
iiØ ∞
UnitId
ii∞ ∂
.
ii∂ ∑
Value
ii∑ º
)
iiº Ω
)
iiΩ æ
.
iiæ ø
ToList
iiø ≈
(
ii≈ ∆
)
ii∆ «
;
ii« »
varkk 
warpMaterialskk !
=kk" #
newkk$ '
Listkk( ,
<kk, -
stringkk- 3
>kk3 4
(kk4 5
)kk5 6
;kk6 7
foreachll 
(ll 
varll 
itemll !
inll" $ 
constructionDocumentll% 9
.ll9 :

ListOfWarpll: D
)llD E
{mm 
varnn 
materialnn  
=nn! "#
_yarnDocumentRepositorynn# :
.nn: ;
Findnn; ?
(nn? @
onn@ A
=>nnB D
onnE F
.nnF G
IdentitynnG O
==nnP R
itemnnS W
.nnW X
YarnIdnnX ^
.nn^ _
Valuenn_ d
)nnd e
.nne f
FirstOrDefaultnnf t
(nnt u
)nnu v
;nnv w
ifoo 
(oo 
materialoo  
!=oo! #
nulloo$ (
)oo( )
{pp 
ifqq 
(qq 
!qq 
warpMaterialsqq *
.qq* +
Containsqq+ 3
(qq3 4
materialqq4 <
.qq< =
Nameqq= A
)qqA B
)qqB C
{rr 
warpMaterialsss )
.ss) *
Addss* -
(ss- .
materialss. 6
.ss6 7
Namess7 ;
)ss; <
;ss< =
}tt 
}uu 
}vv 
varxx 
weftMaterialsxx !
=xx" #
newxx$ '
Listxx( ,
<xx, -
stringxx- 3
>xx3 4
(xx4 5
)xx5 6
;xx6 7
foreachyy 
(yy 
varyy 
itemyy !
inyy" $ 
constructionDocumentyy% 9
.yy9 :

ListOfWarpyy: D
)yyD E
{zz 
var{{ 
material{{  
={{! "#
_yarnDocumentRepository{{# :
.{{: ;
Find{{; ?
({{? @
o{{@ A
=>{{B D
o{{E F
.{{F G
Identity{{G O
=={{P R
item{{S W
.{{W X
YarnId{{X ^
.{{^ _
Value{{_ d
){{d e
.{{e f
FirstOrDefault{{f t
({{t u
){{u v
;{{v w
if|| 
(|| 
material||  
!=||! #
null||$ (
)||( )
{}} 
if~~ 
(~~ 
!~~ 
weftMaterials~~ *
.~~* +
Contains~~+ 3
(~~3 4
material~~4 <
.~~< =
Name~~= A
)~~A B
)~~B C
{ 
weftMaterials
ÄÄ )
.
ÄÄ) *
Add
ÄÄ* -
(
ÄÄ- .
material
ÄÄ. 6
.
ÄÄ6 7
Name
ÄÄ7 ;
)
ÄÄ; <
;
ÄÄ< =
}
ÅÅ 
}
ÇÇ 
}
ÉÉ 
var
ÖÖ 
warpType
ÖÖ 
=
ÖÖ 
$str
ÖÖ !
;
ÖÖ! "
foreach
ÜÜ 
(
ÜÜ 
var
ÜÜ 
item
ÜÜ !
in
ÜÜ" $
warpMaterials
ÜÜ% 2
)
ÜÜ2 3
{
áá 
if
àà 
(
àà 
warpType
àà  
==
àà! #
$str
àà$ &
)
àà& '
{
ââ 
warpType
ää  
=
ää! "
item
ää# '
;
ää' (
}
ãã 
else
åå 
{
çç 
warpType
éé  
=
éé! "
warpType
éé# +
+
éé, -
item
éé. 2
;
éé2 3
}
èè 
}
êê 
var
íí 
weftType
íí 
=
íí 
$str
íí !
;
íí! "
foreach
ìì 
(
ìì 
var
ìì 
item
ìì !
in
ìì" $
weftMaterials
ìì% 2
)
ìì2 3
{
îî 
if
ïï 
(
ïï 
weftType
ïï  
==
ïï! #
$str
ïï$ &
)
ïï& '
{
ññ 
weftType
óó  
=
óó! "
item
óó# '
;
óó' (
}
òò 
else
ôô 
{
öö 
weftType
õõ  
=
õõ! "
item
õõ# '
+
õõ( )
item
õõ* .
;
õõ. /
}
úú 
}
ùù 
var
üü 

yarnNumber
üü 
=
üü  
warpType
üü! )
+
üü* +
$str
üü, /
+
üü0 1
weftType
üü2 :
;
üü: ;
if
°° 
(
°° "
constructionDocument
°° (
==
°°) +
null
°°, 0
)
°°0 1
{
¢¢ 
throw
££ 
	Validator
££ #
.
££# $
ErrorValidation
££$ 3
(
££3 4
(
££4 5
$str
££5 L
,
££L M
$str
§§5 i
+
§§j k
order
••5 :
.
••: ;
Identity
••; C
+
••D E
$str
¶¶5 A
)
¶¶A B
)
¶¶B C
;
¶¶C D
}
ßß 
var
©© 
newOrder
©© 
=
©© 
new
©© "$
OrderReportBySearchDto
©©# 9
(
©©9 :
order
©©: ?
,
©©? @"
constructionDocument
©©A U
,
©©U V 
estimationDocument
©©W i
,
©©i j

yarnNumber
©©k u
,
©©u v
unit
©©w {
)
©©{ |
;
©©| }

resultData
´´ 
.
´´ 
Add
´´ 
(
´´ 
newOrder
´´ '
)
´´' (
;
´´( )
}
¨¨ 
await
ÆÆ 
Task
ÆÆ 
.
ÆÆ 
Yield
ÆÆ 
(
ÆÆ 
)
ÆÆ 
;
ÆÆ 
if
∞∞ 
(
∞∞ 
index
∞∞ 
.
∞∞ 
Equals
∞∞ 
(
∞∞ 
true
∞∞ !
)
∞∞! "
)
∞∞" #
{
±± (
OrderProductionPDFTemplate
≤≤ *
pdfTemplate
≤≤+ 6
=
≤≤7 8
new
≤≤9 <(
OrderProductionPDFTemplate
≤≤= W
(
≤≤W X

resultData
≤≤X b
)
≤≤b c
;
≤≤c d
MemoryStream
≥≥ 
stream
≥≥ #
=
≥≥$ %
pdfTemplate
≥≥& 1
.
≥≥1 2!
GeneratePdfTemplate
≥≥2 E
(
≥≥E F
)
≥≥F G
;
≥≥G H
return
¥¥ 
new
¥¥ 
FileStreamResult
¥¥ +
(
¥¥+ ,
stream
¥¥, 2
,
¥¥2 3
$str
¥¥4 E
)
¥¥E F
{
µµ 
FileDownloadName
∂∂ $
=
∂∂% &
string
∂∂' -
.
∂∂- .
Format
∂∂. 4
(
∂∂4 5
$str
∂∂5 W
)
∂∂W X
}
∑∑ 
;
∑∑ 
}
∏∏ 
else
ππ 
{
∫∫ 
return
ªª 
Ok
ªª 
(
ªª 

resultData
ªª $
)
ªª$ %
;
ªª% &
}
ºº 
}
ΩΩ 	
[
øø 	
HttpGet
øø	 
]
øø 
public
¿¿ 
async
¿¿ 
Task
¿¿ 
<
¿¿ 
IActionResult
¿¿ '
>
¿¿' (
Get
¿¿) ,
(
¿¿, -
int
¿¿- 0
page
¿¿1 5
=
¿¿6 7
$num
¿¿8 9
,
¿¿9 :
int
¡¡- 0
size
¡¡1 5
=
¡¡6 7
$num
¡¡8 :
,
¡¡: ;
string
¬¬- 3
order
¬¬4 9
=
¬¬: ;
$str
¬¬< @
,
¬¬@ A
string
√√- 3
keyword
√√4 ;
=
√√< =
null
√√> B
,
√√B C
string
ƒƒ- 3
filter
ƒƒ4 :
=
ƒƒ; <
$str
ƒƒ= A
)
ƒƒA B
{
≈≈ 	
page
∆∆ 
=
∆∆ 
page
∆∆ 
-
∆∆ 
$num
∆∆ 
;
∆∆ 
var
«« 
query
«« 
=
«« -
_weavingOrderDocumentRepository
»» /
.
»»/ 0
Query
»»0 5
.
»»5 6
OrderByDescending
»»6 G
(
»»G H
item
»»H L
=>
»»M O
item
»»P T
.
»»T U
CreatedDate
»»U `
)
»»` a
;
»»a b
var
…… #
weavingOrderDocuments
…… %
=
……& '-
_weavingOrderDocumentRepository
   /
.
  / 0
Find
  0 4
(
  4 5
query
  5 :
)
  : ;
;
  ; <
var
ÃÃ 

resultData
ÃÃ 
=
ÃÃ 
new
ÃÃ  
List
ÃÃ! %
<
ÃÃ% &)
ListWeavingOrderDocumentDto
ÃÃ& A
>
ÃÃA B
(
ÃÃB C
)
ÃÃC D
;
ÃÃD E
foreach
ŒŒ 
(
ŒŒ 
var
ŒŒ 
weavingOrder
ŒŒ %
in
ŒŒ& (#
weavingOrderDocuments
ŒŒ) >
)
ŒŒ> ?
{
œœ 
var
–– 
construction
––  
=
––! "-
_constructionDocumentRepository
—— 3
.
——3 4
Find
——4 8
(
——8 9
o
——9 :
=>
——; =
o
——> ?
.
——? @
Identity
——@ H
==
——I K
weavingOrder
——L X
.
——X Y
ConstructionId
——Y g
.
——g h
Value
——h m
)
——m n
.
——n o
FirstOrDefault
——o }
(
——} ~
)
——~ 
;—— Ä
var
”” 
	orderData
”” 
=
”” 
new
‘‘ )
ListWeavingOrderDocumentDto
‘‘ 3
(
‘‘3 4
weavingOrder
‘‘4 @
,
‘‘@ A
new
’’4 7(
FabricConstructionDocument
’’8 R
(
’’R S
construction
’’S _
.
’’_ `
Identity
’’` h
,
’’h i
construction
’’j v
.
’’v w!
ConstructionNumber’’w â
)’’â ä
)’’ä ã
;’’ã å

resultData
◊◊ 
.
◊◊ 
Add
◊◊ 
(
◊◊ 
	orderData
◊◊ (
)
◊◊( )
;
◊◊) *
}
ÿÿ 
if
€€ 
(
€€ 
!
€€ 
string
€€ 
.
€€ 
IsNullOrEmpty
€€ %
(
€€% &
keyword
€€& -
)
€€- .
)
€€. /
{
‹‹ 

resultData
›› 
=
›› 

resultData
ﬁﬁ 
.
ﬂﬂ 
Where
ﬂﬂ 
(
ﬂﬂ 
entity
ﬂﬂ %
=>
ﬂﬂ& (
entity
ﬂﬂ) /
.
ﬂﬂ/ 0
OrderNumber
ﬂﬂ0 ;
.
ﬂﬂ; <
Contains
ﬂﬂ< D
(
ﬂﬂD E
keyword
ﬂﬂE L
,
ﬂﬂL M
StringComparison
‡‡E U
.
‡‡U V
OrdinalIgnoreCase
‡‡V g
)
‡‡g h
||
‡‡i k
entity
··) /
.
··/ 0 
ConstructionNumber
··0 B
.
··B C
Contains
··C K
(
··K L
keyword
··L S
,
··S T
StringComparison
‚‚L \
.
‚‚\ ]
OrdinalIgnoreCase
‚‚] n
)
‚‚n o
||
‚‚p r
entity
„„) /
.
„„/ 0
UnitId
„„0 6
.
„„6 7
Value
„„7 <
.
„„< =
ToString
„„= E
(
„„E F
)
„„F G
.
„„G H
Contains
„„H P
(
„„P Q
keyword
„„Q X
,
„„X Y
StringComparison
‰‰J Z
.
‰‰Z [
OrdinalIgnoreCase
‰‰[ l
)
‰‰l m
||
‰‰n p
entity
ÂÂ) /
.
ÂÂ/ 0
DateOrdered
ÂÂ0 ;
.
ÂÂ; <
LocalDateTime
ÂÂ< I
.
ÊÊ; <
ToString
ÊÊ< D
(
ÊÊD E
$str
ÊÊE S
)
ÊÊS T
.
ÁÁ; <
Contains
ÁÁ< D
(
ÁÁD E
keyword
ÁÁE L
,
ÁÁL M
StringComparison
ÁÁN ^
.
ÁÁ^ _
OrdinalIgnoreCase
ÁÁ_ p
)
ÁÁp q
)
ÁÁq r
.
ÁÁr s
ToList
ÁÁs y
(
ÁÁy z
)
ÁÁz {
;
ÁÁ{ |
}
ËË 
if
ÍÍ 
(
ÍÍ 
!
ÍÍ 
order
ÍÍ 
.
ÍÍ 
Contains
ÍÍ 
(
ÍÍ  
$str
ÍÍ  $
)
ÍÍ$ %
)
ÍÍ% &
{
ÎÎ 

Dictionary
ÏÏ 
<
ÏÏ 
string
ÏÏ !
,
ÏÏ! "
string
ÏÏ# )
>
ÏÏ) *
orderDictionary
ÏÏ+ :
=
ÏÏ; <
JsonConvert
ÌÌ 
.
ÌÌ  
DeserializeObject
ÌÌ  1
<
ÌÌ1 2

Dictionary
ÌÌ2 <
<
ÌÌ< =
string
ÌÌ= C
,
ÌÌC D
string
ÌÌE K
>
ÌÌK L
>
ÌÌL M
(
ÌÌM N
order
ÌÌN S
)
ÌÌS T
;
ÌÌT U
var
ÓÓ 
key
ÓÓ 
=
ÓÓ 
orderDictionary
ÓÓ )
.
ÓÓ) *
Keys
ÓÓ* .
.
ÓÓ. /
First
ÓÓ/ 4
(
ÓÓ4 5
)
ÓÓ5 6
.
ÓÓ6 7
	Substring
ÓÓ7 @
(
ÓÓ@ A
$num
ÓÓA B
,
ÓÓB C
$num
ÓÓD E
)
ÓÓE F
.
ÓÓF G
ToUpper
ÓÓG N
(
ÓÓN O
)
ÓÓO P
+
ÓÓQ R
orderDictionary
ÔÔ )
.
ÔÔ) *
Keys
ÔÔ* .
.
ÔÔ. /
First
ÔÔ/ 4
(
ÔÔ4 5
)
ÔÔ5 6
.
ÔÔ6 7
	Substring
ÔÔ7 @
(
ÔÔ@ A
$num
ÔÔA B
)
ÔÔB C
;
ÔÔC D
System
 
.
 

Reflection
 !
.
! "
PropertyInfo
" .
prop
/ 3
=
4 5
typeof
ÒÒ 
(
ÒÒ )
ListWeavingOrderDocumentDto
ÒÒ 6
)
ÒÒ6 7
.
ÒÒ7 8
GetProperty
ÒÒ8 C
(
ÒÒC D
key
ÒÒD G
)
ÒÒG H
;
ÒÒH I
if
ÛÛ 
(
ÛÛ 
orderDictionary
ÛÛ #
.
ÛÛ# $
Values
ÛÛ$ *
.
ÛÛ* +
Contains
ÛÛ+ 3
(
ÛÛ3 4
$str
ÛÛ4 9
)
ÛÛ9 :
)
ÛÛ: ;
{
ÙÙ 

resultData
ıı 
=
ıı  

resultData
ˆˆ "
.
ˆˆ" #
OrderBy
ˆˆ# *
(
ˆˆ* +
x
ˆˆ+ ,
=>
ˆˆ- /
prop
ˆˆ0 4
.
ˆˆ4 5
GetValue
ˆˆ5 =
(
ˆˆ= >
x
ˆˆ> ?
,
ˆˆ? @
null
ˆˆA E
)
ˆˆE F
)
ˆˆF G
.
ˆˆG H
ToList
ˆˆH N
(
ˆˆN O
)
ˆˆO P
;
ˆˆP Q
}
˜˜ 
else
¯¯ 
{
˘˘ 

resultData
˙˙ 
=
˙˙  

resultData
˚˚ "
.
˚˚" #
OrderByDescending
˚˚# 4
(
˚˚4 5
x
˚˚5 6
=>
˚˚7 9
prop
˚˚: >
.
˚˚> ?
GetValue
˚˚? G
(
˚˚G H
x
˚˚H I
,
˚˚I J
null
˚˚K O
)
˚˚O P
)
˚˚P Q
.
˚˚Q R
ToList
˚˚R X
(
˚˚X Y
)
˚˚Y Z
;
˚˚Z [
}
¸¸ 
}
˝˝ 

resultData
ˇˇ 
=
ˇˇ 

resultData
ÄÄ 
.
ÄÄ 
Skip
ÄÄ 
(
ÄÄ  
page
ÄÄ  $
*
ÄÄ% &
size
ÄÄ' +
)
ÄÄ+ ,
.
ÄÄ, -
Take
ÄÄ- 1
(
ÄÄ1 2
size
ÄÄ2 6
)
ÄÄ6 7
.
ÄÄ7 8
ToList
ÄÄ8 >
(
ÄÄ> ?
)
ÄÄ? @
;
ÄÄ@ A
int
ÅÅ 
	totalRows
ÅÅ 
=
ÅÅ 

resultData
ÅÅ &
.
ÅÅ& '
Count
ÅÅ' ,
(
ÅÅ, -
)
ÅÅ- .
;
ÅÅ. /
page
ÇÇ 
=
ÇÇ 
page
ÇÇ 
+
ÇÇ 
$num
ÇÇ 
;
ÇÇ 
await
ÑÑ 
Task
ÑÑ 
.
ÑÑ 
Yield
ÑÑ 
(
ÑÑ 
)
ÑÑ 
;
ÑÑ 
return
ÜÜ 
Ok
ÜÜ 
(
ÜÜ 

resultData
ÜÜ  
,
ÜÜ  !
info
ÜÜ" &
:
ÜÜ& '
new
ÜÜ( +
{
áá 
page
àà 
,
àà 
size
ââ 
,
ââ 
total
ää 
=
ää 
	totalRows
ää !
}
ãã 
)
ãã 
;
ãã 
}
åå 	
[
éé 	
HttpGet
éé	 
(
éé 
$str
éé 
)
éé 
]
éé 
public
èè 
async
èè 
Task
èè 
<
èè 
IActionResult
èè '
>
èè' (
Get
èè) ,
(
èè, -
string
èè- 3
Id
èè4 6
)
èè6 7
{
êê 	
var
ëë 
orderId
ëë 
=
ëë 
Guid
ëë 
.
ëë 
Parse
ëë $
(
ëë$ %
Id
ëë% '
)
ëë' (
;
ëë( )
var
íí 
order
íí 
=
íí -
_weavingOrderDocumentRepository
ìì /
.
ìì/ 0
Find
ìì0 4
(
ìì4 5
item
ìì5 9
=>
ìì: <
item
ìì= A
.
ììA B
Identity
ììB J
==
ììK M
orderId
ììN U
)
ììU V
.
îî/ 0
FirstOrDefault
îî0 >
(
îî> ?
)
îî? @
;
îî@ A
var
ïï 
construction
ïï 
=
ïï -
_constructionDocumentRepository
ññ /
.
ññ/ 0
Find
ññ0 4
(
ññ4 5
item
ññ5 9
=>
ññ: <
item
ññ= A
.
ññA B
Identity
ññB J
==
ññK M
order
ññN S
.
ññS T
ConstructionId
ññT b
.
ññb c
Value
ññc h
)
ññh i
.
óó/ 0
FirstOrDefault
óó0 >
(
óó> ?
)
óó? @
;
óó@ A
var
òò 
orderDto
òò 
=
òò 
new
òò %
WeavingOrderDocumentDto
òò 6
(
òò6 7
order
òò7 <
,
òò< =
new
ôô7 :
UnitId
ôô; A
(
ôôA B
order
ôôB G
.
ôôG H
UnitId
ôôH N
.
ôôN O
Value
ôôO T
)
ôôT U
,
ôôU V
new
öö7 :(
FabricConstructionDocument
öö; U
(
ööU V
construction
ööV b
.
ööb c
Identity
ööc k
,
öök l
construction
õõV b
.
õõb c 
ConstructionNumber
õõc u
)
õõu v
)
õõv w
;
õõw x
await
ùù 
Task
ùù 
.
ùù 
Yield
ùù 
(
ùù 
)
ùù 
;
ùù 
if
üü 
(
üü 
orderId
üü 
==
üü 
null
üü 
)
üü  
{
†† 
return
°° 
	NoContent
°°  
(
°°  !
)
°°! "
;
°°" #
}
¢¢ 
else
££ 
{
§§ 
return
•• 
Ok
•• 
(
•• 
orderDto
•• "
)
••" #
;
••# $
}
¶¶ 
}
ßß 	
[
©© 	
HttpPost
©©	 
]
©© 
public
™™ 
async
™™ 
Task
™™ 
<
™™ 
IActionResult
™™ '
>
™™' (
Post
™™) -
(
™™- .
[
™™. /
FromBody
™™/ 7
]
™™7 8
PlaceOrderCommand
™™8 I
command
™™J Q
)
™™Q R
{
´´ 	
var
¨¨ 
order
¨¨ 
=
¨¨ 
await
¨¨ 
Mediator
¨¨ &
.
¨¨& '
Send
¨¨' +
(
¨¨+ ,
command
¨¨, 3
)
¨¨3 4
;
¨¨4 5
return
ÆÆ 
Ok
ÆÆ 
(
ÆÆ 
order
ÆÆ 
.
ÆÆ 
Identity
ÆÆ $
)
ÆÆ$ %
;
ÆÆ% &
}
ØØ 	
[
±± 	
HttpPut
±±	 
(
±± 
$str
±± 
)
±± 
]
±± 
public
≤≤ 
async
≤≤ 
Task
≤≤ 
<
≤≤ 
IActionResult
≤≤ '
>
≤≤' (
Put
≤≤) ,
(
≤≤, -
string
≤≤- 3
Id
≤≤4 6
,
≤≤6 7
[
≥≥- .
FromBody
≥≥. 6
]
≥≥6 7 
UpdateOrderCommand
≥≥7 I
command
≥≥J Q
)
≥≥Q R
{
¥¥ 	
if
µµ 
(
µµ 
!
µµ 
Guid
µµ 
.
µµ 
TryParse
µµ 
(
µµ 
Id
µµ !
,
µµ! "
out
µµ# &
Guid
µµ' +
orderId
µµ, 3
)
µµ3 4
)
µµ4 5
{
∂∂ 
return
∑∑ 
NotFound
∑∑ 
(
∑∑  
)
∑∑  !
;
∑∑! "
}
∏∏ 
command
∫∫ 
.
∫∫ 
SetId
∫∫ 
(
∫∫ 
orderId
∫∫ !
)
∫∫! "
;
∫∫" #
var
ªª 
order
ªª 
=
ªª 
await
ªª 
Mediator
ªª &
.
ªª& '
Send
ªª' +
(
ªª+ ,
command
ªª, 3
)
ªª3 4
;
ªª4 5
return
ΩΩ 
Ok
ΩΩ 
(
ΩΩ 
order
ΩΩ 
.
ΩΩ 
Identity
ΩΩ $
)
ΩΩ$ %
;
ΩΩ% &
}
ææ 	
[
¿¿ 	

HttpDelete
¿¿	 
(
¿¿ 
$str
¿¿ 
)
¿¿ 
]
¿¿ 
public
¡¡ 
async
¡¡ 
Task
¡¡ 
<
¡¡ 
IActionResult
¡¡ '
>
¡¡' (
Delete
¡¡) /
(
¡¡/ 0
string
¡¡0 6
Id
¡¡7 9
)
¡¡9 :
{
¬¬ 	
if
√√ 
(
√√ 
!
√√ 
Guid
√√ 
.
√√ 
TryParse
√√ 
(
√√ 
Id
√√ !
,
√√! "
out
√√# &
Guid
√√' +
orderId
√√, 3
)
√√3 4
)
√√4 5
{
ƒƒ 
return
≈≈ 
NotFound
≈≈ 
(
≈≈  
)
≈≈  !
;
≈≈! "
}
∆∆ 
var
»» 
command
»» 
=
»» 
new
»»  
RemoveOrderCommand
»» 0
(
»»0 1
)
»»1 2
;
»»2 3
command
…… 
.
…… 
SetId
…… 
(
…… 
orderId
…… !
)
……! "
;
……" #
var
ÀÀ 
order
ÀÀ 
=
ÀÀ 
await
ÀÀ 
Mediator
ÀÀ &
.
ÀÀ& '
Send
ÀÀ' +
(
ÀÀ+ ,
command
ÀÀ, 3
)
ÀÀ3 4
;
ÀÀ4 5
return
ÕÕ 
Ok
ÕÕ 
(
ÕÕ 
order
ÕÕ 
.
ÕÕ 
Identity
ÕÕ $
)
ÕÕ$ %
;
ÕÕ% &
}
ŒŒ 	
}
œœ 
}–– Ç
eD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Controllers\Api\RouterAttribute.cs
	namespace 	
Manufactures
 
. 
Controllers "
." #
Api# &
{ 
internal 
class 
RouterAttribute "
:# $
	Attribute% .
{ 
} 
} ©á
eD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Controllers\Api\ShiftController.cs
	namespace 	
Manufactures
 
. 
Controllers "
." #
Api# &
{ 
[ 
Produces 
( 
$str  
)  !
]! "
[ 
Route 

(
 
$str 
) 
] 
[ 
ApiController 
] 
[ 
	Authorize 
] 
public 

class 
ShiftController  
:! "
ControllerApiBase# 4
{ 
private 
readonly 
IShiftRepository )
_shiftRepository* :
;: ;
public 
ShiftController 
( 
IServiceProvider /
serviceProvider0 ?
,? @
IWorkContext +
workContext, 7
)7 8
:9 :
base; ?
(? @
serviceProvider@ O
)O P
{ 	
_shiftRepository 
= 
this 
. 
Storage 
. 
GetRepository )
<) *
IShiftRepository* :
>: ;
(; <
)< =
;= >
} 	
[!! 	
HttpGet!!	 
]!! 
public"" 
async"" 
Task"" 
<"" 
IActionResult"" '
>""' (
Get"") ,
("", -
int""- 0
page""1 5
=""6 7
$num""8 9
,""9 :
int##, /
size##0 4
=##5 6
$num##7 9
,##9 :
string$$, 2
order$$3 8
=$$9 :
$str$$; ?
,$$? @
string%%, 2
keyword%%3 :
=%%; <
null%%= A
,%%A B
string&&, 2
filter&&3 9
=&&: ;
$str&&< @
)&&@ A
{'' 	
page(( 
=(( 
page(( 
-(( 
$num(( 
;(( 
var)) 
query)) 
=)) 
_shiftRepository**  
.**  !
Query**! &
.**& '
OrderByDescending**' 8
(**8 9
item**9 =
=>**> @
item**A E
.**E F
CreatedDate**F Q
)**Q R
;**R S
var++ 
shiftDocuments++ 
=++  
_shiftRepository,,  
.,,  !
Find,,! %
(,,% &
query,,& +
),,+ ,
.,,, -
Select,,- 3
(,,3 4
x,,4 5
=>,,6 8
new,,9 <
ShiftDto,,= E
(,,E F
x,,F G
),,G H
),,H I
;,,I J
if.. 
(.. 
!.. 
string.. 
... 
IsNullOrEmpty.. %
(..% &
keyword..& -
)..- .
)... /
{// 
shiftDocuments00 
=00  
shiftDocuments11 "
.22 
Where22 
(22 
entity22 %
=>22& (
entity22) /
.22/ 0
Name220 4
.33/ 0
Contains330 8
(338 9
keyword339 @
,33@ A
StringComparison449 I
.44I J
OrdinalIgnoreCase44J [
)44[ \
)44\ ]
.55 
ToList55 
(55  
)55  !
;55! "
}66 
if88 
(88 
!88 
order88 
.88 
Contains88 
(88  
$str88  $
)88$ %
)88% &
{99 

Dictionary:: 
<:: 
string:: !
,::! "
string::# )
>::) *
orderDictionary::+ :
=::; <
JsonConvert;; 
.;;  
DeserializeObject;;  1
<;;1 2

Dictionary;;2 <
<;;< =
string;;= C
,;;C D
string;;E K
>;;K L
>;;L M
(;;M N
order;;N S
);;S T
;;;T U
var<< 
key<< 
=<< 
orderDictionary<< )
.<<) *
Keys<<* .
.<<. /
First<</ 4
(<<4 5
)<<5 6
.<<6 7
	Substring<<7 @
(<<@ A
$num<<A B
,<<B C
$num<<D E
)<<E F
.<<F G
ToUpper<<G N
(<<N O
)<<O P
+<<Q R
orderDictionary== )
.==) *
Keys==* .
.==. /
First==/ 4
(==4 5
)==5 6
.==6 7
	Substring==7 @
(==@ A
$num==A B
)==B C
;==C D
System>> 
.>> 

Reflection>> !
.>>! "
PropertyInfo>>" .
prop>>/ 3
=>>4 5
typeof>>6 <
(>>< =
ShiftDto>>= E
)>>E F
.>>F G
GetProperty>>G R
(>>R S
key>>S V
)>>V W
;>>W X
if@@ 
(@@ 
orderDictionary@@ #
.@@# $
Values@@$ *
.@@* +
Contains@@+ 3
(@@3 4
$str@@4 9
)@@9 :
)@@: ;
{AA 
shiftDocumentsBB "
=BB# $
shiftDocumentsCC &
.CC& '
OrderByCC' .
(CC. /
xCC/ 0
=>CC1 3
propCC4 8
.CC8 9
GetValueCC9 A
(CCA B
xCCB C
,CCC D
nullCCE I
)CCI J
)CCJ K
.CCK L
ToListCCL R
(CCR S
)CCS T
;CCT U
}DD 
elseEE 
{FF 
shiftDocumentsGG "
=GG# $
shiftDocumentsHH &
.HH& '
OrderByDescendingHH' 8
(HH8 9
xHH9 :
=>HH; =
propHH> B
.HHB C
GetValueHHC K
(HHK L
xHHL M
,HHM N
nullHHO S
)HHS T
)HHT U
.HHU V
ToListHHV \
(HH\ ]
)HH] ^
;HH^ _
}II 
}JJ 
shiftDocumentsLL 
=LL 
shiftDocumentsLL +
.LL+ ,
SkipLL, 0
(LL0 1
pageLL1 5
*LL6 7
sizeLL8 <
)LL< =
.LL= >
TakeLL> B
(LLB C
sizeLLC G
)LLG H
.LLH I
ToListLLI O
(LLO P
)LLP Q
;LLQ R
intMM 
	totalRowsMM 
=MM 
shiftDocumentsMM *
.MM* +
CountMM+ 0
(MM0 1
)MM1 2
;MM2 3
pageNN 
=NN 
pageNN 
+NN 
$numNN 
;NN 
varPP 
resultDocumentsPP 
=PP  !
shiftDocumentsPP" 0
;PP0 1
awaitRR 
TaskRR 
.RR 
YieldRR 
(RR 
)RR 
;RR 
returnTT 
OkTT 
(TT 
resultDocumentsTT %
,TT% &
infoTT' +
:TT+ ,
newTT- 0
{UU 
pageVV 
,VV 
sizeWW 
,WW 
totalXX 
=XX 
	totalRowsXX !
}YY 
)YY 
;YY 
}ZZ 	
[\\ 	
HttpGet\\	 
(\\ 
$str\\ 
)\\ 
]\\ 
public]] 
async]] 
Task]] 
<]] 
IActionResult]] '
>]]' (
Get]]) ,
(]], -
string]]- 3
Id]]4 6
)]]6 7
{^^ 	
var__ 
Identity__ 
=__ 
Guid__ 
.__  
Parse__  %
(__% &
Id__& (
)__( )
;__) *
var`` 
shiftDocument`` 
=`` 
_shiftRepositoryaa  
.bb 
Findbb 
(bb 
itembb 
=>bb !
itembb" &
.bb& '
Identitybb' /
==bb0 2
Identitybb3 ;
)bb; <
.cc 
FirstOrDefaultcc #
(cc# $
)cc$ %
;cc% &
awaitdd 
Taskdd 
.dd 
Yielddd 
(dd 
)dd 
;dd 
ifff 
(ff 
shiftDocumentff 
==ff  
nullff! %
)ff% &
{gg 
returnhh 
NotFoundhh 
(hh  
)hh  !
;hh! "
}ii 
elsejj 
{kk 
varll 

resultDatall 
=ll  
newll! $
ShiftDtoll% -
(ll- .
shiftDocumentll. ;
)ll; <
;ll< =
returnnn 
Oknn 
(nn 

resultDatann $
)nn$ %
;nn% &
}oo 
}pp 	
[rr 	
HttpGetrr	 
(rr 
$strrr %
)rr% &
]rr& '
publicss 
asyncss 
Taskss 
<ss 
IActionResultss '
>ss' (

CheckShiftss) 3
(ss3 4
stringss4 :
timess; ?
)ss? @
{tt 	
varuu 
	checkTimeuu 
=uu 
TimeSpanuu $
.uu$ %
Parseuu% *
(uu* +
timeuu+ /
)uu/ 0
;uu0 1
varvv 
defaultTimevv 
=vv 
	checkTimevv '
;vv' (
varww 
queryww 
=ww 
_shiftRepositoryww (
.ww( )
Queryww) .
;ww. /
varxx 
existingShiftxx 
=xx 
_shiftRepositoryyy  
.zz 
Findzz 
(zz 
queryzz 
)zz  
.{{ 
Select{{ 
({{ 
shift{{ !
=>{{" $
new{{% (
ShiftDto{{) 1
({{1 2
shift{{2 7
){{7 8
){{8 9
;{{9 :
foreach}} 
(}} 
var}} 
shift}} 
in}} !
existingShift}}" /
)}}/ 0
{~~ 
	checkTime 
= 
defaultTime '
;' (
var
ÄÄ 
shiftEnd
ÄÄ 
=
ÄÄ 
new
ÄÄ "
TimeSpan
ÄÄ# +
(
ÄÄ+ ,
)
ÄÄ, -
;
ÄÄ- .
var
ÅÅ 

isMoreDays
ÅÅ 
=
ÅÅ  
false
ÅÅ! &
;
ÅÅ& '
if
ÉÉ 
(
ÉÉ 
shift
ÉÉ 
.
ÉÉ 
	StartTime
ÉÉ #
>
ÉÉ$ %
shift
ÉÉ& +
.
ÉÉ+ ,
EndTime
ÉÉ, 3
)
ÉÉ3 4
{
ÑÑ 
if
ÖÖ 
(
ÖÖ 
	checkTime
ÖÖ !
<
ÖÖ" #
shift
ÖÖ$ )
.
ÖÖ) *
	StartTime
ÖÖ* 3
)
ÖÖ3 4
{
ÜÜ 
	checkTime
àà !
=
àà" #
	checkTime
àà$ -
+
àà. /
TimeSpan
àà0 8
.
àà8 9
	FromHours
àà9 B
(
ààB C
$num
ààC E
)
ààE F
;
ààF G
}
ââ 
shiftEnd
åå 
=
åå 
shift
åå $
.
åå$ %
EndTime
åå% ,
+
åå- .
TimeSpan
åå/ 7
.
åå7 8
	FromHours
åå8 A
(
ååA B
$num
ååB D
)
ååD E
;
ååE F

isMoreDays
çç 
=
çç  
true
çç! %
;
çç% &
}
éé 
if
êê 
(
êê 
	checkTime
êê 
>=
êê  
shift
êê! &
.
êê& '
	StartTime
êê' 0
)
êê0 1
{
ëë 
if
íí 
(
íí 

isMoreDays
íí "
==
íí# %
false
íí& +
)
íí+ ,
{
ìì 
if
îî 
(
îî 
	checkTime
îî %
<=
îî& (
shift
îî) .
.
îî. /
EndTime
îî/ 6
)
îî6 7
{
ïï 
await
ññ !
Task
ññ" &
.
ññ& '
Yield
ññ' ,
(
ññ, -
)
ññ- .
;
ññ. /
return
óó "
Ok
óó# %
(
óó% &
shift
óó& +
)
óó+ ,
;
óó, -
}
òò 
}
ôô 
else
öö 
{
õõ 
if
úú 
(
úú 
	checkTime
úú %
<=
úú& (
shiftEnd
úú) 1
)
úú1 2
{
ùù 
await
ûû !
Task
ûû" &
.
ûû& '
Yield
ûû' ,
(
ûû, -
)
ûû- .
;
ûû. /
return
üü "
Ok
üü# %
(
üü% &
shift
üü& +
)
üü+ ,
;
üü, -
}
†† 
}
°° 
}
¢¢ 
}
££ 
await
•• 
Task
•• 
.
•• 
Yield
•• 
(
•• 
)
•• 
;
•• 
return
¶¶ 
NotFound
¶¶ 
(
¶¶ 
)
¶¶ 
;
¶¶ 
}
ßß 	
[
©© 	
HttpPost
©©	 
]
©© 
public
™™ 
async
™™ 
Task
™™ 
<
™™ 
IActionResult
™™ '
>
™™' (
Post
™™) -
(
™™- .
[
™™. /
FromBody
™™/ 7
]
™™7 8
AddShiftCommand
™™8 G
command
™™H O
)
™™O P
{
´´ 	
var
¨¨ 
existingOperator
¨¨  
=
¨¨! "
_shiftRepository
≠≠  
.
ÆÆ 
Query
ÆÆ 
.
ØØ 
Where
ØØ 
(
ØØ 
o
ØØ 
=>
ØØ 
o
ØØ  !
.
ØØ! "
Name
ØØ" &
.
ØØ& '
Equals
ØØ' -
(
ØØ- .
command
ØØ. 5
.
ØØ5 6
Name
ØØ6 :
)
ØØ: ;
)
ØØ; <
.
∞∞ 
FirstOrDefault
∞∞ #
(
∞∞# $
)
∞∞$ %
;
∞∞% &
if
≤≤ 
(
≤≤ 
existingOperator
≤≤  
!=
≤≤! #
null
≤≤$ (
)
≤≤( )
{
≥≥ 
throw
¥¥ 
	Validator
¥¥ 
.
¥¥  
ErrorValidation
¥¥  /
(
¥¥/ 0
(
¥¥0 1
$str
¥¥1 7
,
¥¥7 8
$str
¥¥9 \
)
¥¥\ ]
)
¥¥] ^
;
¥¥^ _
}
µµ 
var
∑∑ 
shiftDocument
∑∑ 
=
∑∑ 
await
∑∑  %
Mediator
∑∑& .
.
∑∑. /
Send
∑∑/ 3
(
∑∑3 4
command
∑∑4 ;
)
∑∑; <
;
∑∑< =
return
ππ 
Ok
ππ 
(
ππ 
shiftDocument
ππ #
.
ππ# $
Identity
ππ$ ,
)
ππ, -
;
ππ- .
}
∫∫ 	
[
ºº 	
HttpPut
ºº	 
(
ºº 
$str
ºº 
)
ºº 
]
ºº 
public
ΩΩ 
async
ΩΩ 
Task
ΩΩ 
<
ΩΩ 
IActionResult
ΩΩ '
>
ΩΩ' (
Put
ΩΩ) ,
(
ΩΩ, -
string
ΩΩ- 3
Id
ΩΩ4 6
,
ΩΩ6 7
[
ææ- .
FromBody
ææ. 6
]
ææ6 7 
UpdateShiftCommand
ææ7 I
command
ææJ Q
)
ææQ R
{
øø 	
if
¿¿ 
(
¿¿ 
!
¿¿ 
Guid
¿¿ 
.
¿¿ 
TryParse
¿¿ 
(
¿¿ 
Id
¿¿ !
,
¿¿! "
out
¿¿# &
Guid
¿¿' +
Identity
¿¿, 4
)
¿¿4 5
)
¿¿5 6
{
¡¡ 
return
¬¬ 
NotFound
¬¬ 
(
¬¬  
)
¬¬  !
;
¬¬! "
}
√√ 
command
≈≈ 
.
≈≈ 
SetId
≈≈ 
(
≈≈ 
Identity
≈≈ "
)
≈≈" #
;
≈≈# $
var
∆∆ 
shiftDocument
∆∆ 
=
∆∆ 
await
∆∆  %
Mediator
∆∆& .
.
∆∆. /
Send
∆∆/ 3
(
∆∆3 4
command
∆∆4 ;
)
∆∆; <
;
∆∆< =
return
»» 
Ok
»» 
(
»» 
shiftDocument
»» #
.
»»# $
Identity
»»$ ,
)
»», -
;
»»- .
}
…… 	
[
ÀÀ 	

HttpDelete
ÀÀ	 
(
ÀÀ 
$str
ÀÀ 
)
ÀÀ 
]
ÀÀ 
public
ÃÃ 
async
ÃÃ 
Task
ÃÃ 
<
ÃÃ 
IActionResult
ÃÃ '
>
ÃÃ' (
Delete
ÃÃ) /
(
ÃÃ/ 0
string
ÃÃ0 6
Id
ÃÃ7 9
)
ÃÃ9 :
{
ÕÕ 	
if
ŒŒ 
(
ŒŒ 
!
ŒŒ 
Guid
ŒŒ 
.
ŒŒ 
TryParse
ŒŒ 
(
ŒŒ 
Id
ŒŒ !
,
ŒŒ! "
out
ŒŒ# &
Guid
ŒŒ' +
Identity
ŒŒ, 4
)
ŒŒ4 5
)
ŒŒ5 6
{
œœ 
return
–– 
NotFound
–– 
(
––  
)
––  !
;
––! "
}
—— 
var
”” 
command
”” 
=
”” 
new
””  
RemoveShiftCommand
”” 0
(
””0 1
)
””1 2
;
””2 3
command
‘‘ 
.
‘‘ 
SetId
‘‘ 
(
‘‘ 
Identity
‘‘ "
)
‘‘" #
;
‘‘# $
var
’’ 
shiftDocument
’’ 
=
’’ 
await
’’  %
Mediator
’’& .
.
’’. /
Send
’’/ 3
(
’’3 4
command
’’4 ;
)
’’; <
;
’’< =
return
◊◊ 
Ok
◊◊ 
(
◊◊ 
shiftDocument
◊◊ #
.
◊◊# $
Identity
◊◊$ ,
)
◊◊, -
;
◊◊- .
}
ÿÿ 	
}
ŸŸ 
}⁄⁄ Á^
hD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Controllers\Api\SupplierController.cs
	namespace 	
Manufactures
 
. 
Controllers "
." #
Api# &
{ 
[ 
Produces 
( 
$str  
)  !
]! "
[ 
Route 

(
 
$str 
) 
]  
[ 
ApiController 
] 
[ 
	Authorize 
] 
public 

class 
SupplierController #
:$ %
ControllerApiBase& 7
{ 
private 
readonly &
IWeavingSupplierRepository 3&
_weavingSupplierRepository4 N
;N O
public 
SupplierController !
(! "
IServiceProvider" 2
serviceProvider3 B
,B C
IWorkContext" .
workContext/ :
): ;
:< =
base> B
(B C
serviceProviderC R
)R S
{ 	&
_weavingSupplierRepository &
=' (
this 
. 
Storage 
. 
GetRepository *
<* +&
IWeavingSupplierRepository+ E
>E F
(F G
)G H
;H I
} 	
[ 	
HttpGet	 
] 
public   
async   
Task   
<   
IActionResult   '
>  ' (
Get  ) ,
(  , -
int  - 0
page  1 5
=  6 7
$num  8 9
,  9 :
int!!- 0
size!!1 5
=!!6 7
$num!!8 :
,!!: ;
string""- 3
order""4 9
="": ;
$str""< @
,""@ A
string##- 3
keyword##4 ;
=##< =
null##> B
,##B C
string$$- 3
filter$$4 :
=$$; <
$str$$= A
)$$A B
{%% 	
page&& 
=&& 
page&& 
-&& 
$num&& 
;&& 
var'' 
query'' 
='' &
_weavingSupplierRepository(( *
.((* +
Query((+ 0
.((0 1
OrderByDescending((1 B
(((B C
item((C G
=>((H J
item((K O
.((O P
CreatedDate((P [
)(([ \
;((\ ]
var)) 
	suppliers)) 
=)) &
_weavingSupplierRepository** *
.*** +
Find**+ /
(**/ 0
query**0 5
)**5 6
.++* +
Select+++ 1
(++1 2
item++2 6
=>++7 9
new++: =
SupplierListDto++> M
(++M N
item++N R
)++R S
)++S T
;++T U
if-- 
(-- 
!-- 
string-- 
.-- 
IsNullOrEmpty-- %
(--% &
keyword--& -
)--- .
)--. /
{.. 
	suppliers// 
=// 
	suppliers00 
.00 
Where00 #
(00# $
entity00$ *
=>00+ -
entity00. 4
.004 5
Code005 9
.009 :
Contains00: B
(00B C
keyword00C J
,00J K
StringComparison11C S
.11S T
OrdinalIgnoreCase11T e
)11e f
||11g i
entity22. 4
.224 5
Name225 9
.229 :
Contains22: B
(22B C
keyword22C J
,22J K
StringComparison33C S
.33S T
OrdinalIgnoreCase33T e
)33e f
)33f g
;33g h
}44 
if66 
(66 
!66 
order66 
.66 
Contains66 
(66  
$str66  $
)66$ %
)66% &
{77 

Dictionary88 
<88 
string88 !
,88! "
string88# )
>88) *
orderDictionary88+ :
=88; <
JsonConvert99 
.99  
DeserializeObject99  1
<991 2

Dictionary992 <
<99< =
string99= C
,99C D
string99E K
>99K L
>99L M
(99M N
order99N S
)99S T
;99T U
var:: 
key:: 
=:: 
orderDictionary:: )
.::) *
Keys::* .
.::. /
First::/ 4
(::4 5
)::5 6
.::6 7
	Substring::7 @
(::@ A
$num::A B
,::B C
$num::D E
)::E F
.::F G
ToUpper::G N
(::N O
)::O P
+::Q R
orderDictionary;; )
.;;) *
Keys;;* .
.;;. /
First;;/ 4
(;;4 5
);;5 6
.;;6 7
	Substring;;7 @
(;;@ A
$num;;A B
);;B C
;;;C D
System<< 
.<< 

Reflection<< !
.<<! "
PropertyInfo<<" .
prop<</ 3
=<<4 5
typeof<<6 <
(<<< =
SupplierListDto<<= L
)<<L M
.<<M N
GetProperty<<N Y
(<<Y Z
key<<Z ]
)<<] ^
;<<^ _
if>> 
(>> 
orderDictionary>> #
.>># $
Values>>$ *
.>>* +
Contains>>+ 3
(>>3 4
$str>>4 9
)>>9 :
)>>: ;
{?? 
	suppliers@@ 
=@@ 
	suppliers@@  )
.@@) *
OrderBy@@* 1
(@@1 2
x@@2 3
=>@@4 6
prop@@7 ;
.@@; <
GetValue@@< D
(@@D E
x@@E F
,@@F G
null@@H L
)@@L M
)@@M N
;@@N O
}AA 
elseBB 
{CC 
	suppliersDD 
=DD 
	suppliersDD  )
.DD) *
OrderByDescendingDD* ;
(DD; <
xDD< =
=>DD> @
propDDA E
.DDE F
GetValueDDF N
(DDN O
xDDO P
,DDP Q
nullDDR V
)DDV W
)DDW X
;DDX Y
}EE 
}FF 
	suppliersHH 
=HH 
	suppliersHH !
.HH! "
SkipHH" &
(HH& '
pageHH' +
*HH, -
sizeHH. 2
)HH2 3
.HH3 4
TakeHH4 8
(HH8 9
sizeHH9 =
)HH= >
;HH> ?
intII 
	totalRowsII 
=II 
	suppliersII %
.II% &
CountII& +
(II+ ,
)II, -
;II- .
pageJJ 
=JJ 
pageJJ 
+JJ 
$numJJ 
;JJ 
awaitLL 
TaskLL 
.LL 
YieldLL 
(LL 
)LL 
;LL 
returnNN 
OkNN 
(NN 
	suppliersNN 
,NN  
infoNN! %
:NN% &
newNN' *
{OO 
pagePP 
,PP 
sizeQQ 
,QQ 
totalRR 
=RR 
	totalRowsRR !
}SS 
)SS 
;SS 
}TT 	
[VV 	
HttpGetVV	 
(VV 
$strVV 
)VV 
]VV 
publicWW 
asyncWW 
TaskWW 
<WW 
IActionResultWW '
>WW' (
GetWW) ,
(WW, -
stringWW- 3
IdWW4 6
)WW6 7
{XX 	
varYY 
IdentityYY 
=YY 
GuidYY 
.YY  
ParseYY  %
(YY% &
IdYY& (
)YY( )
;YY) *
varZZ 
supplierZZ 
=ZZ &
_weavingSupplierRepository[[ *
.[[* +
Find[[+ /
([[/ 0
item[[0 4
=>[[5 7
item[[8 <
.[[< =
Identity[[= E
==[[F H
Identity[[I Q
)[[Q R
.\\* +
Select\\+ 1
(\\1 2
item\\2 6
=>\\7 9
new\\: =
SupplierDocumentDto\\> Q
(\\Q R
item\\R V
)\\V W
)\\W X
.]]* +
FirstOrDefault]]+ 9
(]]9 :
)]]: ;
;]]; <
await^^ 
Task^^ 
.^^ 
Yield^^ 
(^^ 
)^^ 
;^^ 
if`` 
(`` 
supplier`` 
==`` 
null``  
)``  !
{aa 
returnbb 
NotFoundbb 
(bb  
)bb  !
;bb! "
}cc 
elsedd 
{ee 
returnff 
Okff 
(ff 
supplierff "
)ff" #
;ff# $
}gg 
}hh 	
[jj 	
HttpPostjj	 
]jj 
publickk 
asynckk 
Taskkk 
<kk 
IActionResultkk '
>kk' (
Postkk) -
(kk- .
[kk. /
FromBodykk/ 7
]kk7 8#
PlaceNewSupplierCommandkk8 O
commandkkP W
)kkW X
{ll 	
varmm 
SupplierDocumentmm  
=mm! "
awaitmm# (
Mediatormm) 1
.mm1 2
Sendmm2 6
(mm6 7
commandmm7 >
)mm> ?
;mm? @
returnoo 
Okoo 
(oo 
SupplierDocumentoo &
.oo& '
Identityoo' /
)oo/ 0
;oo0 1
}pp 	
[rr 	
HttpPutrr	 
(rr 
$strrr 
)rr 
]rr 
publicss 
asyncss 
Taskss 
<ss 
IActionResultss '
>ss' (
Putss) ,
(ss, -
stringss- 3
Idss4 6
,ss6 7
[tt- .
FromBodytt. 6
]tt6 7*
UpdateExsistingSupplierCommandtt7 U
commandttV ]
)tt] ^
{uu 	
ifvv 
(vv 
!vv 
Guidvv 
.vv 
TryParsevv 
(vv 
Idvv !
,vv! "
outvv# &
Guidvv' +
Identityvv, 4
)vv4 5
)vv5 6
{ww 
returnxx 
NotFoundxx 
(xx  
)xx  !
;xx! "
}yy 
command{{ 
.{{ 
SetId{{ 
({{ 
Identity{{ "
){{" #
;{{# $
var|| 
supplierDocument||  
=||! "
await||# (
Mediator||) 1
.||1 2
Send||2 6
(||6 7
command||7 >
)||> ?
;||? @
return~~ 
Ok~~ 
(~~ 
supplierDocument~~ &
.~~& '
Identity~~' /
)~~/ 0
;~~0 1
} 	
[
ÅÅ 	

HttpDelete
ÅÅ	 
(
ÅÅ 
$str
ÅÅ 
)
ÅÅ 
]
ÅÅ 
public
ÇÇ 
async
ÇÇ 
Task
ÇÇ 
<
ÇÇ 
IActionResult
ÇÇ '
>
ÇÇ' (
Delete
ÇÇ) /
(
ÇÇ/ 0
string
ÇÇ0 6
Id
ÇÇ7 9
)
ÇÇ9 :
{
ÉÉ 	
if
ÑÑ 
(
ÑÑ 
!
ÑÑ 
Guid
ÑÑ 
.
ÑÑ 
TryParse
ÑÑ 
(
ÑÑ 
Id
ÑÑ !
,
ÑÑ! "
out
ÑÑ# &
Guid
ÑÑ' +
Identity
ÑÑ, 4
)
ÑÑ4 5
)
ÑÑ5 6
{
ÖÖ 
return
ÜÜ 
NotFound
ÜÜ 
(
ÜÜ  
)
ÜÜ  !
;
ÜÜ! "
}
áá 
var
ââ 
command
ââ 
=
ââ 
new
ââ #
RemoveSupplierCommand
ââ 3
(
ââ3 4
)
ââ4 5
;
ââ5 6
command
ää 
.
ää 
SetId
ää 
(
ää 
Identity
ää "
)
ää" #
;
ää# $
var
åå 
SupplierDocument
åå  
=
åå! "
await
åå# (
Mediator
åå) 1
.
åå1 2
Send
åå2 6
(
åå6 7
command
åå7 >
)
åå> ?
;
åå? @
return
éé 
Ok
éé 
(
éé 
SupplierDocument
éé &
.
éé& '
Identity
éé' /
)
éé/ 0
;
éé0 1
}
èè 	
}
êê 
}ëë ¨ë
lD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Controllers\Api\YarnDocumentController.cs
	namespace 	
Manufactures
 
. 
Controllers "
." #
Api# &
{ 
[ 
Produces 
( 
$str  
)  !
]! "
[ 
Route 

(
 
$str 
) 
] 
[ 
ApiController 
] 
[ 
	Authorize 
] 
public 

class "
YarnDocumentController '
:( )
ControllerApiBase* ;
{ 
public 
readonly #
IYarnDocumentRepository /#
_yarnDocumentRepository0 G
;G H
public 
readonly #
IMaterialTypeRepository /#
_materialTypeRepository0 G
;G H
public 
readonly !
IYarnNumberRepository -!
_yarnNumberRepository. C
;C D
public "
YarnDocumentController %
(% &
IServiceProvider& 6
serviceProvider7 F
,F G
IWorkContext& 2
workContext3 >
)> ?
:@ A
baseB F
(F G
serviceProviderG V
)V W
{   	#
_yarnDocumentRepository!! #
=!!$ %
this"" 
."" 
Storage"" 
."" 
GetRepository"" *
<""* +#
IYarnDocumentRepository""+ B
>""B C
(""C D
)""D E
;""E F#
_materialTypeRepository## #
=##$ %
this$$ 
.$$ 
Storage$$ 
.$$ 
GetRepository$$ *
<$$* +#
IMaterialTypeRepository$$+ B
>$$B C
($$C D
)$$D E
;$$E F!
_yarnNumberRepository%% !
=%%" #
this&& 
.&& 
Storage&& 
.&& 
GetRepository&& *
<&&* +!
IYarnNumberRepository&&+ @
>&&@ A
(&&A B
)&&B C
;&&C D
}'' 	
[)) 	
HttpGet))	 
])) 
public** 
async** 
Task** 
<** 
IActionResult** '
>**' (
Get**) ,
(**, -
int**- 0
page**1 5
=**6 7
$num**8 9
,**9 :
int++- 0
size++1 5
=++6 7
$num++8 :
,++: ;
string,,- 3
order,,4 9
=,,: ;
$str,,< @
,,,@ A
string--- 3
keyword--4 ;
=--< =
null--> B
,--B C
string..- 3
filter..4 :
=..; <
$str..= A
)..A B
{// 	
page00 
=00 
page00 
-00 
$num00 
;00 
var11 
query11 
=11 #
_yarnDocumentRepository22 '
.22' (
Query22( -
.22- .
OrderByDescending22. ?
(22? @
item22@ D
=>22E G
item22H L
.22L M
CreatedDate22M X
)22X Y
;22Y Z
var33 
yarns33 
=33 #
_yarnDocumentRepository44 '
.44' (
Find44( ,
(44, -
query44- 2
)442 3
;443 4
var66 
results66 
=66 
new66 
List66 "
<66" #
YarnDocumentListDto66# 6
>666 7
(667 8
)668 9
;669 :
foreach88 
(88 
var88 
yarn88 
in88 
yarns88  %
)88% &
{99 
var;; 
materialType;;  
=;;! "#
_materialTypeRepository<< +
.<<+ ,
Find<<, 0
(<<0 1
o<<1 2
=><<3 5
o<<6 7
.<<7 8
Identity<<8 @
==<<A C
yarn<<D H
.<<H I
MaterialTypeId<<I W
.<<W X
Value<<X ]
)<<] ^
.==+ ,
Select==, 2
(==2 3
x==3 4
=>==5 7
new==8 ;#
MaterialTypeValueObject==< S
(==S T
x==T U
.==U V
Identity==V ^
,==^ _
x==` a
.==a b
Code==b f
,==f g
x==h i
.==i j
Name==j n
)==n o
)==o p
.>>+ ,
FirstOrDefault>>, :
(>>: ;
)>>; <
;>>< =
var?? 

yarnNumber?? 
=??  !
_yarnNumberRepository@@ )
.@@) *
Find@@* .
(@@. /
o@@/ 0
=>@@1 3
o@@4 5
.@@5 6
Identity@@6 >
==@@? A
yarn@@B F
.@@F G
YarnNumberId@@G S
.@@S T
Value@@T Y
)@@Y Z
.AA) *
SelectAA* 0
(AA0 1
xAA1 2
=>AA3 5
newAA6 9!
YarnNumberValueObjectAA: O
(AAO P
xAAP Q
.AAQ R
IdentityAAR Z
,AAZ [
xAA\ ]
.AA] ^
CodeAA^ b
,AAb c
xAAd e
.AAe f
NumberAAf l
,AAl m
xAAn o
.AAo p
RingTypeAAp x
)AAx y
)AAy z
.BB) *
FirstOrDefaultBB* 8
(BB8 9
)BB9 :
;BB: ;
varDD 
dataDD 
=DD 
newDD 
YarnDocumentListDtoDD 2
(DD2 3
yarnDD3 7
,DD7 8
materialTypeDD9 E
,DDE F

yarnNumberDDG Q
)DDQ R
;DDR S
resultsFF 
.FF 
AddFF 
(FF 
dataFF  
)FF  !
;FF! "
}GG 
ifJJ 
(JJ 
!JJ 
stringJJ 
.JJ 
IsNullOrEmptyJJ %
(JJ% &
keywordJJ& -
)JJ- .
)JJ. /
{KK 
resultsLL 
=LL 
resultsMM 
.MM 
WhereMM !
(MM! "
entityMM" (
=>MM) +
entityMM, 2
.MM2 3
CodeMM3 7
.MM7 8
ContainsMM8 @
(MM@ A
keywordMMA H
,MMH I
StringComparisonMMJ Z
.MMZ [
OrdinalIgnoreCaseMM[ l
)MMl m
||MMn p
entityNN* 0
.NN0 1
NameNN1 5
.NN5 6
ContainsNN6 >
(NN> ?
keywordNN? F
,NNF G
StringComparisonNNH X
.NNX Y
OrdinalIgnoreCaseNNY j
)NNj k
)NNk l
.NNl m
ToListNNm s
(NNs t
)NNt u
;NNu v
}OO 
ifQQ 
(QQ 
!QQ 
orderQQ 
.QQ 
ContainsQQ 
(QQ  
$strQQ  $
)QQ$ %
)QQ% &
{RR 

DictionarySS 
<SS 
stringSS !
,SS! "
stringSS# )
>SS) *
orderDictionarySS+ :
=SS; <
JsonConvertTT 
.TT  
DeserializeObjectTT  1
<TT1 2

DictionaryTT2 <
<TT< =
stringTT= C
,TTC D
stringTTE K
>TTK L
>TTL M
(TTM N
orderTTN S
)TTS T
;TTT U
varUU 
keyUU 
=UU 
orderDictionaryUU )
.UU) *
KeysUU* .
.UU. /
FirstUU/ 4
(UU4 5
)UU5 6
.UU6 7
	SubstringUU7 @
(UU@ A
$numUUA B
,UUB C
$numUUD E
)UUE F
.UUF G
ToUpperUUG N
(UUN O
)UUO P
+UUQ R
orderDictionaryVV )
.VV) *
KeysVV* .
.VV. /
FirstVV/ 4
(VV4 5
)VV5 6
.VV6 7
	SubstringVV7 @
(VV@ A
$numVVA B
)VVB C
;VVC D
SystemWW 
.WW 

ReflectionWW !
.WW! "
PropertyInfoWW" .
propWW/ 3
=WW4 5
typeofWW6 <
(WW< =
YarnDocumentListDtoWW= P
)WWP Q
.WWQ R
GetPropertyWWR ]
(WW] ^
keyWW^ a
)WWa b
;WWb c
ifYY 
(YY 
orderDictionaryYY #
.YY# $
ValuesYY$ *
.YY* +
ContainsYY+ 3
(YY3 4
$strYY4 9
)YY9 :
)YY: ;
{ZZ 
results[[ 
=[[ 
results[[ %
.[[% &
OrderBy[[& -
([[- .
x[[. /
=>[[0 2
prop[[3 7
.[[7 8
GetValue[[8 @
([[@ A
x[[A B
,[[B C
null[[D H
)[[H I
)[[I J
.[[J K
ToList[[K Q
([[Q R
)[[R S
;[[S T
}\\ 
else]] 
{^^ 
results__ 
=__ 
results__ %
.__% &
OrderByDescending__& 7
(__7 8
x__8 9
=>__: <
prop__= A
.__A B
GetValue__B J
(__J K
x__K L
,__L M
null__N R
)__R S
)__S T
.__T U
ToList__U [
(__[ \
)__\ ]
;__] ^
}`` 
}aa 
resultscc 
=cc 
resultscc 
.cc 
Skipcc "
(cc" #
pagecc# '
*cc( )
sizecc* .
)cc. /
.cc/ 0
Takecc0 4
(cc4 5
sizecc5 9
)cc9 :
.cc: ;
ToListcc; A
(ccA B
)ccB C
;ccC D
intdd 
	totalRowsdd 
=dd 
resultsdd #
.dd# $
Countdd$ )
(dd) *
)dd* +
;dd+ ,
pageee 
=ee 
pageee 
+ee 
$numee 
;ee 
awaitgg 
Taskgg 
.gg 
Yieldgg 
(gg 
)gg 
;gg 
returnii 
Okii 
(ii 
resultsii 
,ii 
infoii #
:ii# $
newii% (
{jj 
pagekk 
,kk 
sizell 
,ll 
totalmm 
=mm 
	totalRowsmm !
}nn 
)nn 
;nn 
}oo 	
[qq 	
HttpGetqq	 
(qq 
$strqq 
)qq 
]qq 
publicrr 
asyncrr 
Taskrr 
<rr 
IActionResultrr '
>rr' (
Getrr) ,
(rr, -
stringrr- 3
Idrr4 6
)rr6 7
{ss 	
vartt 
Identitytt 
=tt 
Guidtt 
.tt  
Parsett  %
(tt% &
Idtt& (
)tt( )
;tt) *
varuu 
yarnuu 
=uu #
_yarnDocumentRepositoryvv '
.vv' (
Findvv( ,
(vv, -
itemvv- 1
=>vv2 4
itemvv5 9
.vv9 :
Identityvv: B
==vvC E
IdentityvvF N
)vvN O
.ww' (
FirstOrDefaultww( 6
(ww6 7
)ww7 8
;ww8 9
varxx 
materialTypexx 
=xx #
_materialTypeRepositoryyy '
.yy' (
Findyy( ,
(yy, -
itemyy- 1
=>yy2 4
itemyy5 9
.yy9 :
Identityyy: B
==yyC E
yarnyyF J
.yyJ K
MaterialTypeIdyyK Y
.yyY Z
ValueyyZ _
)yy_ `
.zz' (
Selectzz( .
(zz. /
xzz/ 0
=>zz1 3
newzz4 7#
MaterialTypeValueObjectzz8 O
(zzO P
xzzP Q
.zzQ R
IdentityzzR Z
,zzZ [
xzz\ ]
.zz] ^
Codezz^ b
,zzb c
xzzd e
.zze f
Namezzf j
)zzj k
)zzk l
.{{' (
FirstOrDefault{{( 6
({{6 7
){{7 8
;{{8 9
var|| 
yarnNumberDocument|| "
=||# $!
_yarnNumberRepository}} %
.}}% &
Find}}& *
(}}* +
item}}+ /
=>}}0 2
item}}3 7
.}}7 8
Identity}}8 @
==}}A C
yarn}}D H
.}}H I
YarnNumberId}}I U
.}}U V
Value}}V [
)}}[ \
.~~% &
Select~~& ,
(~~, -
x~~- .
=>~~/ 1
new~~2 5!
YarnNumberValueObject~~6 K
(~~K L
x~~L M
.~~M N
Identity~~N V
,~~V W
x~~X Y
.~~Y Z
Code~~Z ^
,~~^ _
x~~` a
.~~a b
Number~~b h
,~~h i
x~~j k
.~~k l
RingType~~l t
)~~t u
)~~u v
.% &
FirstOrDefault& 4
(4 5
)5 6
;6 7
await
ÄÄ 
Task
ÄÄ 
.
ÄÄ 
Yield
ÄÄ 
(
ÄÄ 
)
ÄÄ 
;
ÄÄ 
if
ÇÇ 
(
ÇÇ 
yarn
ÇÇ 
==
ÇÇ 
null
ÇÇ 
||
ÇÇ 
materialType
ÇÇ  ,
==
ÇÇ- /
null
ÇÇ0 4
||
ÇÇ5 7 
yarnNumberDocument
ÇÇ8 J
==
ÇÇK M
null
ÇÇN R
)
ÇÇR S
{
ÉÉ 
return
ÑÑ 
NotFound
ÑÑ 
(
ÑÑ  
)
ÑÑ  !
;
ÑÑ! "
}
ÖÖ 
else
ÜÜ 
{
áá 
var
àà 
result
àà 
=
àà 
new
àà  
YarnDocumentDto
àà! 0
(
àà0 1
yarn
àà1 5
,
àà5 6
materialType
àà7 C
,
ààC D 
yarnNumberDocument
ààE W
)
ààW X
;
ààX Y
return
ââ 
Ok
ââ 
(
ââ 
result
ââ  
)
ââ  !
;
ââ! "
}
ää 
}
ãã 	
[
çç 	
HttpPost
çç	 
]
çç 
public
éé 
async
éé 
Task
éé 
<
éé 
IActionResult
éé '
>
éé' (
Post
éé) -
(
éé- .
[
éé. /
FromBody
éé/ 7
]
éé7 8"
CreateNewYarnCommand
éé8 L
command
ééM T
)
ééT U
{
èè 	
var
êê 
yarnDocument
êê 
=
êê 
await
êê $
Mediator
êê% -
.
êê- .
Send
êê. 2
(
êê2 3
command
êê3 :
)
êê: ;
;
êê; <
return
íí 
Ok
íí 
(
íí 
yarnDocument
íí "
.
íí" #
Identity
íí# +
)
íí+ ,
;
íí, -
}
ìì 	
[
ïï 	
HttpPut
ïï	 
(
ïï 
$str
ïï 
)
ïï 
]
ïï 
public
ññ 
async
ññ 
Task
ññ 
<
ññ 
IActionResult
ññ '
>
ññ' (
Put
ññ) ,
(
ññ, -
string
ññ- 3
Id
ññ4 6
,
ññ6 7
[
óó- .
FromBody
óó. 6
]
óó6 7(
UpdateExsistingYarnCommand
óó7 Q
command
óóR Y
)
óóY Z
{
òò 	
if
ôô 
(
ôô 
!
ôô 
Guid
ôô 
.
ôô 
TryParse
ôô 
(
ôô 
Id
ôô !
,
ôô! "
out
ôô# &
Guid
ôô' +
Identity
ôô, 4
)
ôô4 5
)
ôô5 6
{
öö 
return
õõ 
NotFound
õõ 
(
õõ  
)
õõ  !
;
õõ! "
}
úú 
var
ûû 
existingCode
ûû 
=
ûû %
_yarnDocumentRepository
üü '
.
üü' (
Find
üü( ,
(
üü, -
o
üü- .
=>
üü/ 1
o
üü2 3
.
üü3 4
Code
üü4 8
.
üü8 9
Equals
üü9 ?
(
üü? @
command
üü@ G
.
üüG H
Code
üüH L
)
üüL M
&&
üüN P
o
††2 3
.
††3 4
Deleted
††4 ;
.
††; <
Equals
††< B
(
††B C
false
††C H
)
††H I
)
††I J
.
††J K
Count
††K P
>=
††Q S
$num
††T U
;
††U V
if
¢¢ 
(
¢¢ 
existingCode
¢¢ 
)
¢¢ 
{
££ 
throw
§§ 
	Validator
§§ 
.
§§  
ErrorValidation
§§  /
(
§§/ 0
(
§§0 1
$str
§§1 7
,
§§7 8
$str
§§9 E
+
§§F G
command
§§H O
.
§§O P
Code
§§P T
+
§§U V
$str
§§W g
)
§§g h
)
§§h i
;
§§i j
}
•• 
command
ßß 
.
ßß 
SetId
ßß 
(
ßß 
Identity
ßß "
)
ßß" #
;
ßß# $
var
®® 
yarnDocument
®® 
=
®® 
await
®® $
Mediator
®®% -
.
®®- .
Send
®®. 2
(
®®2 3
command
®®3 :
)
®®: ;
;
®®; <
return
™™ 
Ok
™™ 
(
™™ 
yarnDocument
™™ "
.
™™" #
Identity
™™# +
)
™™+ ,
;
™™, -
}
´´ 	
[
≠≠ 	

HttpDelete
≠≠	 
(
≠≠ 
$str
≠≠ 
)
≠≠ 
]
≠≠ 
public
ÆÆ 
async
ÆÆ 
Task
ÆÆ 
<
ÆÆ 
IActionResult
ÆÆ '
>
ÆÆ' (
Delete
ÆÆ) /
(
ÆÆ/ 0
string
ÆÆ0 6
Id
ÆÆ7 9
)
ÆÆ9 :
{
ØØ 	
if
∞∞ 
(
∞∞ 
!
∞∞ 
Guid
∞∞ 
.
∞∞ 
TryParse
∞∞ 
(
∞∞ 
Id
∞∞ !
,
∞∞! "
out
∞∞# &
Guid
∞∞' +
Identity
∞∞, 4
)
∞∞4 5
)
∞∞5 6
{
±± 
return
≤≤ 
NotFound
≤≤ 
(
≤≤  
)
≤≤  !
;
≤≤! "
}
≥≥ 
var
µµ 
command
µµ 
=
µµ 
new
µµ (
RemoveExsistingYarnCommand
µµ 8
(
µµ8 9
)
µµ9 :
;
µµ: ;
command
∂∂ 
.
∂∂ 
SetId
∂∂ 
(
∂∂ 
Identity
∂∂ "
)
∂∂" #
;
∂∂# $
var
∏∏ 
yarnDocument
∏∏ 
=
∏∏ 
await
∏∏ $
Mediator
∏∏% -
.
∏∏- .
Send
∏∏. 2
(
∏∏2 3
command
∏∏3 :
)
∏∏: ;
;
∏∏; <
return
∫∫ 
Ok
∫∫ 
(
∫∫ 
yarnDocument
∫∫ "
.
∫∫" #
Identity
∫∫# +
)
∫∫+ ,
;
∫∫, -
}
ªª 	
}
ºº 
}ΩΩ ¨]
rD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Controllers\Api\YarnNumberDocumentController.cs
	namespace 	
Manufactures
 
. 
Controllers "
." #
Api# &
{ 
[ 
Produces 
( 
$str  
)  !
]! "
[ 
Route 

(
 
$str !
)! "
]" #
[ 
ApiController 
] 
[ 
	Authorize 
] 
public 

class (
YarnNumberDocumentController -
:. /
ControllerApiBase0 A
{ 
private 
readonly !
IYarnNumberRepository .!
_yarnNumberRepository/ D
;D E
public (
YarnNumberDocumentController +
(+ ,
IServiceProvider, <
serviceProvider= L
,L M
IWorkContext& 2
workContext3 >
)> ?
:@ A
baseB F
(F G
serviceProviderG V
)V W
{ 	!
_yarnNumberRepository !
=" #
this 
. 
Storage 
. 
GetRepository *
<* +!
IYarnNumberRepository+ @
>@ A
(A B
)B C
;C D
} 	
[ 	
HttpGet	 
] 
public   
async   
Task   
<   
IActionResult   '
>  ' (
Get  ) ,
(  , -
int  - 0
page  1 5
=  6 7
$num  8 9
,  9 :
int!!- 0
size!!1 5
=!!6 7
$num!!8 :
,!!: ;
string""- 3
order""4 9
="": ;
$str""< @
,""@ A
string##- 3
keyword##4 ;
=##< =
null##> B
,##B C
string$$- 3
filter$$4 :
=$$; <
$str$$= A
)$$A B
{%% 	
page&& 
=&& 
page&& 
-&& 
$num&& 
;&& 
var'' 
query'' 
='' !
_yarnNumberRepository(( %
.((% &
Query((& +
.((+ ,
OrderByDescending((, =
(((= >
item((> B
=>((C E
item((F J
.((J K
CreatedDate((K V
)((V W
;((W X
var)) 
yarnNumberDocuments)) #
=))$ %!
_yarnNumberRepository** %
.**% &
Find**& *
(*** +
query**+ 0
)**0 1
.++  
Select++  &
(++& '
item++' +
=>++, .
new++/ 2
YarnNumberListDto++3 D
(++D E
item++E I
)++I J
)++J K
;++K L
if-- 
(-- 
!-- 
string-- 
.-- 
IsNullOrEmpty-- %
(--% &
keyword--& -
)--- .
)--. /
{.. 
yarnNumberDocuments// #
=//$ %
yarnNumberDocuments00 '
.00' (
Where00( -
(00- .
entity00. 4
=>005 7
entity008 >
.00> ?
Code00? C
.00C D
Contains00D L
(00L M
keyword00M T
,00T U
StringComparison00V f
.00f g
OrdinalIgnoreCase00g x
)00x y
)00y z
;00z {
}11 
if33 
(33 
!33 
order33 
.33 
Contains33 
(33  
$str33  $
)33$ %
)33% &
{44 

Dictionary55 
<55 
string55 !
,55! "
string55# )
>55) *
orderDictionary55+ :
=55; <
JsonConvert66 
.66  
DeserializeObject66  1
<661 2

Dictionary662 <
<66< =
string66= C
,66C D
string66E K
>66K L
>66L M
(66M N
order66N S
)66S T
;66T U
var77 
key77 
=77 
orderDictionary77 )
.77) *
Keys77* .
.77. /
First77/ 4
(774 5
)775 6
.776 7
	Substring777 @
(77@ A
$num77A B
,77B C
$num77D E
)77E F
.77F G
ToUpper77G N
(77N O
)77O P
+77Q R
orderDictionary88 )
.88) *
Keys88* .
.88. /
First88/ 4
(884 5
)885 6
.886 7
	Substring887 @
(88@ A
$num88A B
)88B C
;88C D
System99 
.99 

Reflection99 !
.99! "
PropertyInfo99" .
prop99/ 3
=994 5
typeof996 <
(99< =
YarnNumberListDto99= N
)99N O
.99O P
GetProperty99P [
(99[ \
key99\ _
)99_ `
;99` a
if;; 
(;; 
orderDictionary;; #
.;;# $
Values;;$ *
.;;* +
Contains;;+ 3
(;;3 4
$str;;4 9
);;9 :
);;: ;
{<< 
yarnNumberDocuments== '
===( )
yarnNumberDocuments==* =
.=== >
OrderBy==> E
(==E F
x==F G
=>==H J
prop==K O
.==O P
GetValue==P X
(==X Y
x==Y Z
,==Z [
null==\ `
)==` a
)==a b
;==b c
}>> 
else?? 
{@@ 
yarnNumberDocumentsAA '
=AA( )
yarnNumberDocumentsAA* =
.AA= >
OrderByDescendingAA> O
(AAO P
xAAP Q
=>AAR T
propAAU Y
.AAY Z
GetValueAAZ b
(AAb c
xAAc d
,AAd e
nullAAf j
)AAj k
)AAk l
;AAl m
}BB 
}CC 
yarnNumberDocumentsEE 
=EE  !
yarnNumberDocumentsEE" 5
.EE5 6
SkipEE6 :
(EE: ;
pageEE; ?
*EE@ A
sizeEEB F
)EEF G
.EEG H
TakeEEH L
(EEL M
sizeEEM Q
)EEQ R
;EER S
intFF 
	totalRowsFF 
=FF 
yarnNumberDocumentsFF /
.FF/ 0
CountFF0 5
(FF5 6
)FF6 7
;FF7 8
pageGG 
=GG 
pageGG 
+GG 
$numGG 
;GG 
awaitII 
TaskII 
.II 
YieldII 
(II 
)II 
;II 
returnKK 
OkKK 
(KK 
yarnNumberDocumentsKK )
,KK) *
infoKK+ /
:KK/ 0
newKK1 4
{LL 
pageMM 
,MM 
sizeNN 
,NN 
totalOO 
=OO 
	totalRowsOO !
}PP 
)PP 
;PP 
}QQ 	
[SS 	
HttpGetSS	 
(SS 
$strSS 
)SS 
]SS 
publicTT 
asyncTT 
TaskTT 
<TT 
IActionResultTT '
>TT' (
GetTT) ,
(TT, -
stringTT- 3
IdTT4 6
)TT6 7
{UU 	
varVV 
IdentityVV 
=VV 
GuidVV 
.VV  
ParseVV  %
(VV% &
IdVV& (
)VV( )
;VV) *
varWW 
ringDocumentsWW 
=WW !
_yarnNumberRepositoryXX %
.XX% &
FindXX& *
(XX* +
itemXX+ /
=>XX0 2
itemXX3 7
.XX7 8
IdentityXX8 @
==XXA C
IdentityXXD L
)XXL M
.YY  
SelectYY  &
(YY& '
itemYY' +
=>YY, .
newYY/ 2!
YarnNumberDocumentDtoYY3 H
(YYH I
itemYYI M
)YYM N
)YYN O
.YYO P
FirstOrDefaultYYP ^
(YY^ _
)YY_ `
;YY` a
awaitZZ 
TaskZZ 
.ZZ 
YieldZZ 
(ZZ 
)ZZ 
;ZZ 
if\\ 
(\\ 
ringDocuments\\ 
==\\  
null\\! %
)\\% &
{]] 
return^^ 
NotFound^^ 
(^^  
)^^  !
;^^! "
}__ 
else`` 
{aa 
returnbb 
Okbb 
(bb 
ringDocumentsbb '
)bb' (
;bb( )
}cc 
}dd 	
[ff 	
HttpPostff	 
]ff 
publicgg 
asyncgg 
Taskgg 
<gg 
IActionResultgg '
>gg' (
Postgg) -
(gg- .
[gg. /
FromBodygg/ 7
]gg7 8#
AddNewYarnNumberCommandgg8 O
commandggP W
)ggW X
{hh 	
varii 
ringDocumentii 
=ii 
awaitii $
Mediatorii% -
.ii- .
Sendii. 2
(ii2 3
commandii3 :
)ii: ;
;ii; <
returnkk 
Okkk 
(kk 
ringDocumentkk "
.kk" #
Identitykk# +
)kk+ ,
;kk, -
}ll 	
[nn 	
HttpPutnn	 
(nn 
$strnn 
)nn 
]nn 
publicoo 
asyncoo 
Taskoo 
<oo 
IActionResultoo '
>oo' (
Putoo) ,
(oo, -
stringoo- 3
Idoo4 6
,oo6 7
[pp- .
FromBodypp. 6
]pp6 7#
UpdateYarnNumberCommandpp7 N
commandppO V
)ppV W
{qq 	
ifrr 
(rr 
!rr 
Guidrr 
.rr 
TryParserr 
(rr 
Idrr !
,rr! "
outrr# &
Guidrr' +
Identityrr, 4
)rr4 5
)rr5 6
{ss 
returntt 
NotFoundtt 
(tt  
)tt  !
;tt! "
}uu 
commandww 
.ww 
SetIdww 
(ww 
Identityww "
)ww" #
;ww# $
varxx 
ringDocumentxx 
=xx 
awaitxx $
Mediatorxx% -
.xx- .
Sendxx. 2
(xx2 3
commandxx3 :
)xx: ;
;xx; <
returnzz 
Okzz 
(zz 
ringDocumentzz "
.zz" #
Identityzz# +
)zz+ ,
;zz, -
}{{ 	
[}} 	

HttpDelete}}	 
(}} 
$str}} 
)}} 
]}} 
public~~ 
async~~ 
Task~~ 
<~~ 
IActionResult~~ '
>~~' (
Delete~~) /
(~~/ 0
string~~0 6
Id~~7 9
)~~9 :
{ 	
if
ÄÄ 
(
ÄÄ 
!
ÄÄ 
Guid
ÄÄ 
.
ÄÄ 
TryParse
ÄÄ 
(
ÄÄ 
Id
ÄÄ !
,
ÄÄ! "
out
ÄÄ# &
Guid
ÄÄ' +
Identity
ÄÄ, 4
)
ÄÄ4 5
)
ÄÄ5 6
{
ÅÅ 
return
ÇÇ 
NotFound
ÇÇ 
(
ÇÇ  
)
ÇÇ  !
;
ÇÇ! "
}
ÉÉ 
var
ÖÖ 
command
ÖÖ 
=
ÖÖ 
new
ÖÖ %
RemoveYarnNumberCommand
ÖÖ 5
(
ÖÖ5 6
)
ÖÖ6 7
;
ÖÖ7 8
command
ÜÜ 
.
ÜÜ 
SetId
ÜÜ 
(
ÜÜ 
Identity
ÜÜ "
)
ÜÜ" #
;
ÜÜ# $
var
àà 
ringDocument
àà 
=
àà 
await
àà $
Mediator
àà% -
.
àà- .
Send
àà. 2
(
àà2 3
command
àà3 :
)
àà: ;
;
àà; <
return
ää 
Ok
ää 
(
ää 
ringDocument
ää "
.
ää" #
Identity
ää# +
)
ää+ ,
;
ää, -
}
ãã 	
}
åå 
}çç ﬂ
gD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Controllers\ManufactureController.cs
	namespace 	
Manufactures
 
. 
Controllers "
{		 
public

 

class

 !
ManufactureController

 &
:

' (
Barebone

) 1
.

1 2
Controllers

2 =
.

= >
ControllerBase

> L
{ 
private 
readonly 
IWorkContext %
_workContext& 2
;2 3
public !
ManufactureController $
($ %
IWorkContext% 1
workContext2 =
,= >
IStorage? G
storageH O
)O P
:Q R
baseS W
(W X
storageX _
)_ `
{ 	
_workContext 
= 
workContext &
;& '
} 	
public 
ActionResult 
Index !
(! "
)" #
{ 	
return 
this 
. 
View 
( 
new  !
IndexViewModelFactory! 6
(6 7
)7 8
.8 9
Create9 ?
(? @
this@ D
.D E
StorageE L
)L M
)M N
;N O
} 	
[ 	
HttpGet	 
] 
public 
ActionResult 
Create "
(" #
)# $
{ 	
return 
this 
. 
View 
( 
) 
; 
} 	
[ 	
HttpPost	 
] 
public 
ActionResult 
Create "
(" #
CreateViewModel# 2
createViewModel3 B
)B C
{   	
if!! 
(!! 
this!! 
.!! 

ModelState!! 
.!!  
IsValid!!  '
)!!' (
{"" 
ManufactureOrder##  
order##! &
=##' (
new##) ,!
CreateViewModelMapper##- B
(##B C
)##C D
.##D E
Map##E H
(##H I
createViewModel##I X
,##X Y
_workContext##Z f
.##f g
CurrentUser##g r
)##r s
;##s t
this%% 
.%% 
Storage%% 
.%% 
GetRepository%% *
<%%* +'
IManufactureOrderRepository%%+ F
>%%F G
(%%G H
)%%H I
.%%I J
Update%%J P
(%%P Q
order%%Q V
)%%V W
.%%W X
Wait%%X \
(%%\ ]
)%%] ^
;%%^ _
this'' 
.'' 
Storage'' 
.'' 
Save'' !
(''! "
)''" #
;''# $
return(( 
this(( 
.(( 
RedirectToAction(( ,
(((, -
$str((- 4
)((4 5
;((5 6
})) 
return++ 
this++ 
.++ 
View++ 
(++ 
)++ 
;++ 
},, 	
}-- 
}.. Æ
XD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\Beams\BeamDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
Beams !
{ 
public 

class 
BeamDto 
{ 
[		 	
JsonProperty			 
(		 
PropertyName		 "
=		# $
$str		% )
)		) *
]		* +
public

 
Guid

 
Id

 
{

 
get

 
;

 
}

 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% -
)- .
]. /
public 
string 
Number 
{ 
get "
;" #
}$ %
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% +
)+ ,
], -
public 
string 
Type 
{ 
get  
;  !
}" #
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 2
)2 3
]3 4
public 
double 
EmptyWeight !
{" #
get$ '
;' (
}) *
public 
BeamDto 
( 
BeamDocument #
document$ ,
), -
{ 	
Id 
= 
document 
. 
Identity "
;" #
Number 
= 
document 
. 
Number $
;$ %
Type 
= 
document 
. 
Type  
;  !
EmptyWeight 
= 
document "
." #
EmptyWeight# .
;. /
} 	
} 
} «
yD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\DailyOperations\Loom\DailyOperationLoomListDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
DailyOperations +
.+ ,
Loom, 0
{ 
public 

class %
DailyOperationLoomListDto *
{		 
[

 	
JsonProperty

	 
(

 
PropertyName

 "
=

# $
$str

% )
)

) *
]

* +
public 
Guid 
Id 
{ 
get 
; 
} 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 3
)3 4
]4 5
public 
DateTimeOffset 
DateOperated *
{+ ,
get- 0
;0 1
}2 3
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 2
)2 3
]3 4
public 
string 
OrderNumber !
{" #
get$ '
;' (
}) *
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% -
)- .
]. /
public 
UnitId 
UnitId 
{ 
get "
;" #
}$ %
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 4
)4 5
]5 6
public 
string 
MachineNumber #
{$ %
get& )
;) *
}+ ,
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% ;
); <
]< =
public 
string  
DailyOperationStatus *
{+ ,
get- 0
;0 1
}2 3
public %
DailyOperationLoomListDto (
(( )&
DailyOperationLoomDocument) C
documentD L
,L M
string) /
orderNumber0 ;
,; <
string) /
machineNumber0 =
,= >
DateTimeOffset) 7
dateOperated8 D
)D E
{   	
Id!! 
=!! 
document!! 
.!! 
Identity!! "
;!!" #
OrderNumber"" 
="" 
orderNumber"" %
;""% &
MachineNumber## 
=## 
machineNumber## )
;##) *
UnitId$$ 
=$$ 
document$$ 
.$$ 
UnitId$$ $
;$$$ % 
DailyOperationStatus%%  
=%%! "
document%%# +
.%%+ , 
DailyOperationStatus%%, @
;%%@ A
DateOperated&& 
=&& 
dateOperated&& '
;&&' (
}'' 	
}(( 
})) È*
}D:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\DailyOperations\Sizing\DailyOperationSizingByIdDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
DailyOperations +
.+ ,
Sizing, 2
{		 
public

 

class

 '
DailyOperationSizingByIdDto

 ,
{ 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% )
)) *
]* +
public 
Guid 
Id 
{ 
get 
; 
} 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 8
)8 9
]9 :
public 
	MachineId 
MachineDocumentId *
{+ ,
get- 0
;0 1
}2 3
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% <
)< =
]= >
public 
UnitId !
WeavingUnitDocumentId +
{, -
get. 1
;1 2
}3 4
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% F
)F G
]G H
public 
List 
< 
BeamId 
> +
WarpingBeamCollectionDocumentId ;
{< =
get> A
;A B
}C D
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% =
)= >
]> ?
public 
ConstructionId "
ConstructionDocumentId 4
{5 6
get7 :
;: ;
}< =
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% .
). /
]/ 0
public *
DailyOperationSizingCounterDto -
Counter. 5
{6 7
get8 ;
;; <
}= >
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% ,
), -
]- .
public 
double 
Visco 
{ 
get !
;! "
}# $
[!! 	
JsonProperty!!	 
(!! 
PropertyName!! "
=!!# $
$str!!% *
)!!* +
]!!+ ,
public"" 
int"" 
PIS"" 
{"" 
get"" 
;"" 
}"" 
[$$ 	
JsonProperty$$	 
($$ 
PropertyName$$ "
=$$# $
$str$$% ;
)$$; <
]$$< =
public%% 
BeamId%%  
SizingBeamDocumentId%% *
{%%+ ,
get%%- 0
;%%0 1
}%%2 3
['' 	
JsonProperty''	 
('' 
PropertyName'' "
=''# $
$str''% B
)''B C
]''C D
public(( 
List(( 
<(( *
DailyOperationSizingDetailsDto(( 2
>((2 3'
DailyOperationSizingDetails((4 O
{((P Q
get((R U
;((U V
}((W X
public** '
DailyOperationSizingByIdDto** *
(*** +(
DailyOperationSizingDocument**+ G
document**H P
)**P Q
{++ 	
Id,, 
=,, 
document,, 
.,, 
Identity,, "
;,," #
MachineDocumentId-- 
=-- 
document--  (
.--( )
MachineDocumentId--) :
;--: ;!
WeavingUnitDocumentId.. !
=.." #
document..$ ,
..., -
WeavingUnitId..- :
;..: ;+
WarpingBeamCollectionDocumentId// +
=//, -
document//. 6
.//6 7+
WarpingBeamCollectionDocumentId//7 V
.//V W
Deserialize//W b
<//b c
List//c g
<//g h
BeamId//h n
>//n o
>//o p
(//p q
)//q r
;//r s"
ConstructionDocumentId00 "
=00# $
document00% -
.00- ."
ConstructionDocumentId00. D
;00D E
Counter11 
=11 
document11 
.11 
Counter11 &
.11& '
Deserialize11' 2
<112 3*
DailyOperationSizingCounterDto113 Q
>11Q R
(11R S
)11S T
;11T U
Visco22 
=22 
document22 
.22 
Visco22 "
;22" #
PIS33 
=33 
document33 
.33 
PIS33 
;33  
SizingBeamDocumentId44  
=44! "
document44# +
.44+ , 
SizingBeamDocumentId44, @
;44@ A
foreach55 
(55 
var55 
details55  
in55! #
document55$ ,
.55, -'
DailyOperationSizingDetails55- H
)55H I
{66 
var77 

detailsDto77 
=77  
new77! $*
DailyOperationSizingDetailsDto77% C
(77C D
details77D K
)77K L
;77L M'
DailyOperationSizingDetails88 +
.88+ ,
Add88, /
(88/ 0

detailsDto880 :
)88: ;
;88; <
}99 
}:: 	
};; 
}<< Õ
D:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\DailyOperations\Sizing\DailyOperationSizingCausesDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
DailyOperations +
.+ ,
Sizing, 2
{ 
public 

class )
DailyOperationSizingCausesDto .
{		 
[

 	
JsonProperty

	 
(

 
PropertyName

 "
=

# $
$str

% 1
)

1 2
]

2 3
public 
string 

BrokenBeam  
{! "
get# &
;& '
}( )
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 6
)6 7
]7 8
public 
string 
MachineTroubled %
{& '
get( +
;+ ,
}- .
} 
} ¬
ÄD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\DailyOperations\Sizing\DailyOperationSizingCounterDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
DailyOperations +
.+ ,
Sizing, 2
{ 
public 

class *
DailyOperationSizingCounterDto /
{		 
[

 	
JsonProperty

	 
(

 
PropertyName

 "
=

# $
$str

% ,
)

, -
]

- .
public 
string 
Start 
{ 
get !
;! "
}# $
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% -
)- .
]. /
public 
string 
Finish 
{ 
get "
;" #
}$ %
} 
} Ú
ÄD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\DailyOperations\Sizing\DailyOperationSizingDetailsDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
DailyOperations +
.+ ,
Sizing, 2
{ 
public		 

class		 *
DailyOperationSizingDetailsDto		 /
{

 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% .
). /
]/ 0
public *
DailyOperationSizingHistoryDto -
History. 5
{6 7
get8 ;
;; <
}= >
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 6
)6 7
]7 8
public 
ShiftId 
ShiftDocumentId &
{' (
get) ,
;, -
}. /
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% -
)- .
]. /
public )
DailyOperationSizingCausesDto ,
Causes- 3
{4 5
get6 9
;9 :
}; <
public *
DailyOperationSizingDetailsDto -
(- .&
DailyOperationSizingDetail. H
detailI O
)O P
{ 	
History 
= 
detail 
. 
History $
.$ %
Deserialize% 0
<0 1*
DailyOperationSizingHistoryDto1 O
>O P
(P Q
)Q R
;R S
ShiftDocumentId 
= 
new !
ShiftId" )
() *
detail* 0
.0 1
ShiftDocumentId1 @
)@ A
;A B
Causes 
= 
detail 
. 
Causes "
." #
Deserialize# .
<. /)
DailyOperationSizingCausesDto/ L
>L M
(M N
)N O
;O P
} 	
} 
} È	
ÄD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\DailyOperations\Sizing\DailyOperationSizingHistoryDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
DailyOperations +
.+ ,
Sizing, 2
{ 
public		 

class		 *
DailyOperationSizingHistoryDto		 /
{

 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 4
)4 5
]5 6
public 
DateTimeOffset 
TimeOnMachine +
{, -
get. 1
;1 2
}4 5
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 4
)4 5
]5 6
public 
string 
MachineStatus #
{$ %
get& )
;) *
}+ ,
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 2
)2 3
]3 4
public 
string 
Information !
{" #
get$ '
;' (
}* +
} 
} ®
}D:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\DailyOperations\Sizing\DailyOperationSizingListDto.cs
	namespace		 	
Manufactures		
 
.		 
Dtos		 
.		 
DailyOperations		 +
.		+ ,
Sizing		, 2
{

 
public 

class '
DailyOperationSizingListDto ,
{ 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% )
)) *
]* +
public 
Guid 
Id 
{ 
get 
; 
} 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 8
)8 9
]9 :
public 
	MachineId 
MachineDocumentId *
{+ ,
get- 0
;0 1
}2 3
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% <
)< =
]= >
public 
UnitId !
WeavingUnitDocumentId +
{, -
get. 1
;1 2
}3 4
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% =
)= >
]> ?
public 
ConstructionId "
ConstructionDocumentId 4
{5 6
get7 :
;: ;
}< =
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% *
)* +
]+ ,
public 
int 
PIS 
{ 
get 
; 
} 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 6
)6 7
]7 8
public 
ShiftId 
ShiftDocumentId &
{' (
get) ,
;, -
}. /
public '
DailyOperationSizingListDto *
(* +(
DailyOperationSizingDocument+ G
documentH P
,P Q&
DailyOperationSizingDetailR l
detailsm t
)t u
{   	
Id!! 
=!! 
document!! 
.!! 
Identity!! "
;!!" #
MachineDocumentId"" 
="" 
document""  (
.""( )
MachineDocumentId"") :
;"": ;!
WeavingUnitDocumentId## !
=##" #
document##$ ,
.##, -
WeavingUnitId##- :
;##: ;"
ConstructionDocumentId$$ "
=$$# $
new$$% (
ConstructionId$$) 7
($$7 8
document$$8 @
.$$@ A"
ConstructionDocumentId$$A W
.$$W X
Value$$X ]
)$$] ^
;$$^ _
PIS%% 
=%% 
document%% 
.%% 
PIS%% 
;%% 
ShiftDocumentId&& 
=&& 
new&& !
ShiftId&&" )
(&&) *
details&&* 1
.&&1 2
ShiftDocumentId&&2 A
)&&A B
;&&B C
}'' 	
}(( 
})) ’"
oD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\EstimationsProduct\EstimationByIdDto.cs
	namespace		 	
Manufactures		
 
.		 
Dtos		 
.		 
EstimationsProduct		 .
{

 
public 

class 
EstimationByIdDto "
{ 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% )
)) *
]* +
public 
Guid 
Id 
{ 
get 
; 
} 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 6
)6 7
]7 8
public 
string 
EstimatedNumber %
{& '
get( +
;+ ,
}- .
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% -
)- .
]. /
public 
Period 
Period 
{ 
get "
;" #
}$ %
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% +
)+ ,
], -
public 
UnitId 
Unit 
{ 
get  
;  !
}" #
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 9
)9 :
]: ;
public 
List 
< (
EstimationProductValueObject 0
>0 1
EstimationProducts2 D
{E F
getG J
;J K
}L M
public 
EstimationByIdDto  
(  !'
EstimatedProductionDocument! <
document= E
)E F
{ 	
Id 
= 
document 
. 
Identity "
;" #
EstimatedNumber 
= 
document &
.& '
EstimatedNumber' 6
;6 7
Period   
=   
document   
.   
Period   $
;  $ %
Unit!! 
=!! 
document!! 
.!! 
UnitId!! "
;!!" #
EstimationProducts"" 
=""  
new""! $
List""% )
<"") *(
EstimationProductValueObject""* F
>""F G
(""G H
)""H I
;""I J
foreach$$ 
($$ 
var$$ 
product$$  
in$$! #
document$$$ ,
.$$, -
EstimationProducts$$- ?
)$$? @
{%% 
var&& 
order&& 
=&& 
product&& #
.&&# $
OrderDocument&&$ 1
.&&1 2
Deserialize&&2 =
<&&= >$
OrderDocumentValueObject&&> V
>&&V W
(&&W X
)&&X Y
;&&Y Z
var'' 
productGrade''  
=''! "
product''# *
.''* +
ProductGrade''+ 7
.''7 8
Deserialize''8 C
<''C D
ProductGrade''D P
>''P Q
(''Q R
)''R S
;''S T
var(( 
obj(( 
=(( 
new(( (
EstimationProductValueObject(( :
(((: ;
order((; @
.((@ A
Construction((A M
.((M N
ConstructionNumber((N `
,((` a
order)); @
.))@ A
Construction))A M
.))M N
	TotalYarn))N W
,))W X
order**; @
.**@ A
DateOrdered**A L
,**L M
order++; @
.++@ A
OrderNumber++A L
,++L M
order,,; @
.,,@ A
AllGrade,,A I
,,,I J
product--; B
.--B C
TotalGramEstimation--C V
,--V W
productGrade..; G
...G H
GradeA..H N
,..N O
productGrade//; G
.//G H
GradeB//H N
,//N O
productGrade00; G
.00G H
GradeC00H N
,00N O
productGrade11; G
.11G H
GradeD11H N
)11N O
;11O P
EstimationProducts33 "
.33" #
Add33# &
(33& '
obj33' *
)33* +
;33+ ,
}44 
}55 	
}66 
}77 Í
oD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\EstimationsProduct\ListEstimationDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
EstimationsProduct .
{ 
public 

class 
ListEstimationDto "
{		 
[

 	
JsonProperty

	 
(

 
PropertyName

 "
=

# $
$str

% )
)

) *
]

* +
public 
Guid 
Id 
{ 
get 
; 
} 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 7
)7 8
]8 9
public 
string 
EstimationNumber &
{' (
get) ,
;, -
}. /
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 4
)4 5
]5 6
public 
string 
DateEstimated #
{$ %
get& )
;) *
}+ ,
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% -
)- .
]. /
public 
Period 
Period 
{ 
get "
;" #
}$ %
public 
ListEstimationDto  
(  !'
EstimatedProductionDocument! <
document= E
)E F
{ 	
Id 
= 
document 
. 
Identity "
;" #
EstimationNumber 
= 
document '
.' (
EstimatedNumber( 7
;7 8
DateEstimated 
= 
document $
.$ %
Date% )
.) *
ToString* 2
(2 3
$str3 B
)B C
;C D
Period 
= 
document 
. 
Period $
;$ %
} 	
} 
} ˘2
xD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\FabricConstructions\FabricConstructionByIdDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
FabricConstructions /
{		 
public

 

class

 %
FabricConstructionByIdDto

 *
{ 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% )
)) *
]* +
public 
Guid 
Id 
{ 
get 
; 
} 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 9
)9 :
]: ;
public 
string 
ConstructionNumber (
{) *
get+ .
;. /
}0 1
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 7
)7 8
]8 9
public 
string 
MaterialTypeName &
{' (
get) ,
;, -
}. /
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 0
)0 1
]1 2
public 
string 
	WovenType 
{  !
get" %
;% &
}' (
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 3
)3 4
]4 5
public 
int 
AmountOfWarp 
{  !
get" %
;% &
}' (
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 3
)3 4
]4 5
public 
int 
AmountOfWeft 
{  !
get" %
;% &
}' (
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% ,
), -
]- .
public 
int 
Width 
{ 
get 
; 
}  !
[!! 	
JsonProperty!!	 
(!! 
PropertyName!! "
=!!# $
$str!!% 3
)!!3 4
]!!4 5
public"" 
string"" 
WarpTypeForm"" "
{""# $
get""% (
;""( )
}""* +
[$$ 	
JsonProperty$$	 
($$ 
PropertyName$$ "
=$$# $
$str$$% 3
)$$3 4
]$$4 5
public%% 
string%% 
WeftTypeForm%% "
{%%# $
get%%% (
;%%( )
}%%* +
['' 	
JsonProperty''	 
('' 
PropertyName'' "
=''# $
$str''% 0
)''0 1
]''1 2
public(( 
double(( 
	TotalYarn(( 
{((  !
get((" %
;((% &
}((' (
[** 	
JsonProperty**	 
(** 
PropertyName** "
=**# $
$str**% 0
)**0 1
]**1 2
public++ 
IReadOnlyCollection++ "
<++" #
Warp++# '
>++' (
	ItemsWarp++) 2
{++3 4
get++5 8
;++8 9
private++: A
set++B E
;++E F
}++G H
[-- 	
JsonProperty--	 
(-- 
PropertyName-- "
=--# $
$str--% 0
)--0 1
]--1 2
public.. 
IReadOnlyCollection.. "
<.." #
Weft..# '
>..' (
	ItemsWeft..) 2
{..3 4
get..5 8
;..8 9
private..: A
set..B E
;..E F
}..G H
public11 %
FabricConstructionByIdDto11 (
(11( )&
FabricConstructionDocument11) C
document11D L
)11L M
{22 	
Id33 
=33 
document33 
.33 
Identity33 "
;33" #
ConstructionNumber44 
=44  
document44! )
.44) *
ConstructionNumber44* <
;44< =
AmountOfWarp55 
=55 
document55 #
.55# $
AmountOfWarp55$ 0
;550 1
AmountOfWeft66 
=66 
document66 #
.66# $
AmountOfWeft66$ 0
;660 1
Width77 
=77 
document77 
.77 
Width77 "
;77" #
	WovenType88 
=88 
document88  
.88  !
	WovenType88! *
;88* +
WarpTypeForm99 
=99 
document99 #
.99# $
WarpType99$ ,
;99, -
WeftTypeForm:: 
=:: 
document:: #
.::# $
WeftType::$ ,
;::, -
	TotalYarn;; 
=;; 
document;;  
.;;  !
	TotalYarn;;! *
;;;* +
MaterialTypeName<< 
=<< 
document<< '
.<<' (
MaterialTypeName<<( 8
;<<8 9
	ItemsWarp>> 
=>> 
new>> 
List>>  
<>>  !
Warp>>! %
>>>% &
(>>& '
)>>' (
;>>( )
	ItemsWeft?? 
=?? 
new?? 
List??  
<??  !
Weft??! %
>??% &
(??& '
)??' (
;??( )
}@@ 	
publicBB 
voidBB 
AddWarpBB 
(BB 
WarpBB  
valueBB! &
)BB& '
{CC 	
varDD 
listDD 
=DD 
	ItemsWarpDD  
.DD  !
ToListDD! '
(DD' (
)DD( )
;DD) *
listEE 
.EE 
AddEE 
(EE 
valueEE 
)EE 
;EE 
	ItemsWarpFF 
=FF 
listFF 
;FF 
}GG 	
publicII 
voidII 
AddWeftII 
(II 
WeftII  
valueII! &
)II& '
{JJ 	
varKK 
listKK 
=KK 
	ItemsWeftKK  
.KK  !
ToListKK! '
(KK' (
)KK( )
;KK) *
listLL 
.LL 
AddLL 
(LL 
valueLL 
)LL 
;LL 
	ItemsWeftMM 
=MM 
listMM 
;MM 
}NN 	
}PP 
}QQ …
|D:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\FabricConstructions\FabricConstructionDocumentDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
FabricConstructions /
{ 
public 

class )
FabricConstructionDocumentDto .
{ 
[		 	
JsonProperty			 
(		 
PropertyName		 "
=		# $
$str		% )
)		) *
]		* +
public

 
Guid

 
Id

 
{

 
get

 
;

 
}

 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 9
)9 :
]: ;
public 
string 
ConstructionNumber (
{) *
get+ .
;. /
}0 1
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% +
)+ ,
], -
public 
string 
Date 
{ 
get  
;  !
}" #
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 0
)0 1
]1 2
public 
double 
	TotalYarn 
{  !
get" %
;% &
}' (
public )
FabricConstructionDocumentDto ,
(, -&
FabricConstructionDocument- G 
constructionDocumentH \
)\ ]
{ 	
Id 
=  
constructionDocument %
.% &
Identity& .
;. /
ConstructionNumber 
=   
constructionDocument! 5
.5 6
ConstructionNumber6 H
;H I
	TotalYarn 
=  
constructionDocument ,
., -
	TotalYarn- 6
;6 7
Date 
=  
constructionDocument '
.' (
Date( ,
., -
ToString- 5
(5 6
$str6 E
)E F
;F G
} 	
} 
} „
wD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\MachinesPlanning\MachinesPlanningDocumentDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
MachinesPlanning ,
{ 
public 

class '
MachinesPlanningDocumentDto ,
{		 
[

 	
JsonProperty

	 
(

 
propertyName

 "
:

" #
$str

$ (
)

( )
]

) *
public 
Guid 
Id 
{ 
get 
; 
} 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% +
)+ ,
], -
public 
string 
Area 
{ 
get  
;  !
}" #
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% +
)+ ,
], -
public 
string 
Blok 
{ 
get  
;  !
}" #
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 1
)1 2
]2 3
public 
string 

BlokKaizen  
{! "
get# &
;& '
}( )
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% .
). /
]/ 0
public 
ManufactureMachine !
Machine" )
{* +
get, /
;/ 0
}1 2
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 8
)8 9
]9 :
public 
UnitId 
UnitDepartementId '
{( )
get* -
;- .
}/ 0
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 8
)8 9
]9 :
public 
UserId 
UserMaintenanceId '
{( )
get* -
;- .
}/ 0
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 5
)5 6
]6 7
public   
UserId   
UserOperatorId   $
{  % &
get  ' *
;  * +
}  , -
public"" '
MachinesPlanningDocumentDto"" *
(""* +$
MachinesPlanningDocument""+ C
document""D L
,""L M
ManufactureMachine##+ =
machine##> E
)##E F
{$$ 	
Id%% 
=%% 
document%% 
.%% 
Identity%% "
;%%" #
Area&& 
=&& 
document&& 
.&& 
Area&&  
;&&  !
Blok'' 
='' 
document'' 
.'' 
Blok''  
;''  !

BlokKaizen(( 
=(( 
document(( !
.((! "

BlokKaizen((" ,
;((, -
Machine)) 
=)) 
machine)) 
;)) 
UnitDepartementId** 
=** 
document**  (
.**( )
UnitDepartementId**) :
;**: ;
UserMaintenanceId++ 
=++ 
document++  (
.++( )
UserMaintenanceId++) :
;++: ;
UserOperatorId,, 
=,, 
document,, %
.,,% &
UserOperatorId,,& 4
;,,4 5
}-- 	
}.. 
}// ≈
sD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\MachinesPlanning\MachinesPlanningListDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
MachinesPlanning ,
{ 
public 

class #
MachinesPlanningListDto (
{		 
[

 	
JsonProperty

	 
(

 
propertyName

 "
:

" #
$str

$ (
)

( )
]

) *
public 
Guid 
Id 
{ 
get 
; 
} 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% +
)+ ,
], -
public 
string 
Area 
{ 
get  
;  !
}" #
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% +
)+ ,
], -
public 
string 
Blok 
{ 
get  
;  !
}" #
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 1
)1 2
]2 3
public 
string 

BlokKaizen  
{! "
get# &
;& '
}( )
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 4
)4 5
]5 6
public 
string 
MachineNumber #
{$ %
get& )
;) *
}+ ,
public #
MachinesPlanningListDto &
(& '$
MachinesPlanningDocument' ?
document@ H
,H I
ManufactureMachine' 9
machine: A
)A B
{ 	
Id 
= 
document 
. 
Identity "
;" #
Area 
= 
document 
. 
Area  
;  !
Blok 
= 
document 
. 
Blok  
;  !

BlokKaizen 
= 
document !
.! "

BlokKaizen" ,
;, -
MachineNumber   
=   
machine   #
.  # $
MachineNumber  $ 1
;  1 2
}!! 	
}"" 
}## Ù
mD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\MachineType\MachineTypeDocumentDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
MachineType '
{ 
public 

class "
MachineTypeDocumentDto '
{ 
[		 	
JsonProperty			 
(		 
propertyName		 "
:		" #
$str		$ (
)		( )
]		) *
public

 
Guid

 
Id

 
{

 
get

 
;

 
}

 
[ 	
JsonProperty	 
( 
propertyName "
:" #
$str$ .
). /
]/ 0
public 
string 
TypeName 
{  
get! $
;$ %
}& '
[ 	
JsonProperty	 
( 
propertyName "
:" #
$str$ +
)+ ,
], -
public 
int 
Speed 
{ 
get 
; 
}  !
[ 	
JsonProperty	 
( 
propertyName "
:" #
$str$ 1
)1 2
]2 3
public 
string 
MachineUnit !
{" #
get$ '
;' (
}) *
public "
MachineTypeDocumentDto %
(% &
MachineTypeDocument& 9
document: B
)B C
{ 	
Id 
= 
document 
. 
Identity "
;" #
TypeName 
= 
document 
.  
TypeName  (
;( )
Speed 
= 
document 
. 
Speed "
;" #
MachineUnit 
= 
document "
." #
MachineUnit# .
;. /
} 	
} 
} Û	
iD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\MachineType\MachineTypeListDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
MachineType '
{ 
public 

class 
MachineTypeListDto #
{ 
[		 	
JsonProperty			 
(		 
propertyName		 "
:		" #
$str		$ (
)		( )
]		) *
public

 
Guid

 
Id

 
{

 
get

 
;

 
}

 
[ 	
JsonProperty	 
( 
propertyName "
:" #
$str$ .
). /
]/ 0
public 
string 
TypeName 
{  
get! $
;$ %
}& '
public 
MachineTypeListDto !
(! "
MachineTypeDocument" 5
document6 >
)> ?
{ 	
Id 
= 
document 
. 
Identity "
;" #
TypeName 
= 
document 
.  
TypeName  (
;( )
} 	
} 
} ®
eD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\Machine\MachineDocumentDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
Machine #
{		 
public

 

class

 
MachineDocumentDto

 #
{ 
[ 	
JsonProperty	 
( 
propertyName "
:" #
$str$ (
)( )
]) *
public 
Guid 
Id 
{ 
get 
; 
} 
[ 	
JsonProperty	 
( 
propertyName "
:" #
$str$ 3
)3 4
]4 5
public 
string 
MachineNumber #
{$ %
get& )
;) *
private+ 2
set3 6
;6 7
}8 9
[ 	
JsonProperty	 
( 
propertyName "
:" #
$str$ .
). /
]/ 0
public 
string 
Location 
{  
get! $
;$ %
private& -
set. 1
;1 2
}3 4
[ 	
JsonProperty	 
( 
propertyName "
:" #
$str$ 3
)3 4
]4 5
public 
MachineTypeId 
MachineTypeId *
{+ ,
get- 0
;0 1
private2 9
set: =
;= >
}? @
[ 	
JsonProperty	 
( 
propertyName "
:" #
$str$ 3
)3 4
]4 5
public 
UnitId 
WeavingUnitId #
{$ %
get& )
;) *
private+ 2
set3 6
;6 7
}8 9
public 
MachineDocumentDto !
(! "
MachineDocument" 1
document2 :
): ;
{ 	
Id 
= 
document 
. 
Identity "
;" #
MachineNumber 
= 
document $
.$ %
MachineNumber% 2
;2 3
Location 
= 
document 
.  
Location  (
;( )
MachineTypeId   
=   
document   $
.  $ %
MachineTypeId  % 2
;  2 3
WeavingUnitId!! 
=!! 
document!! $
.!!$ %
WeavingUnitId!!% 2
;!!2 3
}"" 	
public$$ 
MachineDocumentDto$$ !
($$! "
MachineDocument$$" 1
machine$$2 9
,$$9 :"
MachineTypeValueObject%%" 8"
machineTypeValueObject%%9 O
,%%O P
WeavingUnit&&" -"
weavingUnitValueObject&&. D
)&&D E
{'' 	
})) 	
}** 
}++ Ê
aD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\Machine\MachineListDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
Machine #
{ 
public 

class 
MachineListDto 
{ 
[		 	
JsonProperty			 
(		 
propertyName		 "
:		" #
$str		$ (
)		( )
]		) *
public

 
Guid

 
Id

 
{

 
get

 
;

 
}

 
[ 	
JsonProperty	 
( 
propertyName "
:" #
$str$ 3
)3 4
]4 5
public 
string 
MachineNumber #
{$ %
get& )
;) *
}+ ,
[ 	
JsonProperty	 
( 
propertyName "
:" #
$str$ .
). /
]/ 0
public 
string 
Location 
{  
get! $
;$ %
}& '
public 
MachineListDto 
( 
MachineDocument -
document. 6
)6 7
{ 	
Id 
= 
document 
. 
Identity "
;" #
MachineNumber 
= 
document $
.$ %
MachineNumber% 2
;2 3
Location 
= 
document 
.  
Location  (
;( )
} 	
} 
} é
^D:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\ManufactureOrderDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
{ 
public 

class 
ManufactureOrderDto $
{ 
public		 
ManufactureOrderDto		 "
(		" #
ManufactureOrder		# 3
order		4 9
)		9 :
{

 	
	OrderDate 
= 
order 
. 
	OrderDate '
;' (
UnitDepartmentId 
= 
order $
.$ %
UnitDepartmentId% 5
;5 6
	YarnCodes 
= 
order 
. 
	YarnCodes '
;' (
Blended 
= 
order 
. 
Blended #
;# $
	MachineId 
= 
order 
. 
	MachineId '
;' (
State 
= 
order 
. 
State 
;  
UserId 
= 
order 
. 
UserId !
;! "
Id 
= 
order 
. 
Identity 
;  
LastModifiedDate 
= 
order $
.$ %

AuditTrail% /
./ 0
ModifiedDate0 <
??= ?
order@ E
.E F

AuditTrailF P
.P Q
CreatedDateQ \
;\ ]
LastModifiedBy 
= 
order "
." #

AuditTrail# -
.- .

ModifiedBy. 8
??9 ;
order< A
.A B

AuditTrailB L
.L M
	CreatedByM V
;V W
} 	
public 
Guid 
Id 
{ 
get 
; 
} 
public 
DateTimeOffset 
LastModifiedDate .
{/ 0
get1 4
;4 5
}6 7
public 
string 
LastModifiedBy $
{% &
get' *
;* +
}, -
public 
DateTimeOffset 
	OrderDate '
{( )
get* -
;- .
}/ 0
public   
UnitDepartmentId   
UnitDepartmentId    0
{  1 2
get  3 6
;  6 7
}  8 9
public"" 
	YarnCodes"" 
	YarnCodes"" "
{""# $
get""% (
;""( )
}""* +
public$$ 
Blended$$ 
Blended$$ 
{$$  
get$$! $
;$$$ %
}$$& '
public&&  
MachineIdValueObject&& #
	MachineId&&$ -
{&&. /
get&&0 3
;&&3 4
}&&5 6
public(( 
ManufactureOrder(( 
.((  
Status((  &
State((' ,
{((- .
get((/ 2
;((2 3
}((4 5
public-- 
string-- 
UserId-- 
{-- 
get-- "
;--" #
}--$ %
}.. 
}// ö
oD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\MaterialType\MaterialTypeDocumentDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
MaterialType (
{		 
public

 

class

 #
MaterialTypeDocumentDto

 (
{ 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% )
)) *
]* +
public 
Guid 
Id 
{ 
get 
; 
private %
set& )
;) *
}+ ,
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% +
)+ ,
], -
public 
string 
Code 
{ 
get  
;  !
private" )
set* -
;- .
}/ 0
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% +
)+ ,
], -
public 
string 
Name 
{ 
get  
;  !
private" )
set* -
;- .
}/ 0
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 4
)4 5
]5 6
public 
List 
< !
YarnNumberValueObject )
>) *
RingDocuments+ 8
{9 :
get; >
;> ?
private@ G
setH K
;K L
}M N
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 2
)2 3
]3 4
public 
string 
Description !
{" #
get$ '
;' (
private) 0
set1 4
;4 5
}6 7
public #
MaterialTypeDocumentDto &
(& ' 
MaterialTypeDocument' ;
materialType< H
)H I
{ 	
Id 
= 
materialType 
. 
Identity &
;& '
Code 
= 
materialType 
.  
Code  $
;$ %
Name 
= 
materialType 
.  
Name  $
;$ %
RingDocuments   
=   
materialType   (
.  ( )
RingDocuments  ) 6
.  6 7
ToList  7 =
(  = >
)  > ?
;  ? @
Description!! 
=!! 
materialType!! &
.!!& '
Description!!' 2
;!!2 3
}""	 

}## 
}$$ å
kD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\MaterialType\MaterialTypeListDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
MaterialType (
{ 
public 

class 
MaterialTypeListDto $
{ 
[		 	
JsonProperty			 
(		 
PropertyName		 "
=		# $
$str		% )
)		) *
]		* +
public

 
Guid

 
Id

 
{

 
get

 
;

 
private

 %
set

& )
;

) *
}

+ ,
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% +
)+ ,
], -
public 
string 
Code 
{ 
get  
;  !
private" )
set* -
;- .
}/ 0
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% +
)+ ,
], -
public 
string 
Name 
{ 
get  
;  !
private" )
set* -
;- .
}/ 0
public 
MaterialTypeListDto "
(" # 
MaterialTypeDocument# 7
materialType8 D
)D E
{ 	
Id 
= 
materialType 
. 
Identity &
;& '
Code 
= 
materialType 
.  
Code  $
;$ %
Name 
= 
materialType 
.  
Name  $
;$ %
} 	
} 
} Ø
bD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\Operator\CoreAccountDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
Operator $
{		 
public

 

class

 
CoreAccountDto

 
{ 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% .
). /
]/ 0
public 
string 
MongoId 
{ 
get  #
;# $
}% &
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% )
)) *
]* +
public 
int 
Id 
{ 
get 
; 
} 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% +
)+ ,
], -
public 
string 
Name 
{ 
get  
;  !
}" #
public 
CoreAccountDto 
( 
CoreAccount )
core* .
). /
{ 	
MongoId 
= 
core 
. 
MongoId "
;" #
Id 
= 
core 
. 
Id 
; 
Name 
= 
core 
. 
Name 
; 
} 	
} 
} ò
cD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\Operator\OperatorByIdDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
Operator $
{		 
public

 

class

 
OperatorByIdDto

  
{ 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% )
)) *
]* +
public 
Guid 
Id 
{ 
get 
; 
} 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 2
)2 3
]3 4
public 
CoreAccountDto 
CoreAccount )
{* +
get, /
;/ 0
}1 2
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% -
)- .
]. /
public 
UnitId 
UnitId 
{ 
get "
;" #
}$ %
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% ,
), -
]- .
public 
string 
Group 
{ 
get !
;! "
}# $
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 1
)1 2
]2 3
public 
string 

Assignment  
{! "
get# &
;& '
}( )
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% +
)+ ,
], -
public 
string 
Type 
{ 
get  
;  !
}" #
public 
OperatorByIdDto 
( 
OperatorDocument /
document0 8
)8 9
{ 	
Id   
=   
document   
.   
Identity   "
;  " #
CoreAccount!! 
=!! 
new!! 
CoreAccountDto!! ,
(!!, -
document!!- 5
.!!5 6
CoreAccount!!6 A
)!!A B
;!!B C
UnitId"" 
="" 
document"" 
."" 
UnitId"" $
;""$ %
Group## 
=## 
document## 
.## 
Group## "
;##" #

Assignment$$ 
=$$ 
document$$ !
.$$! "

Assignment$$" ,
;$$, -
Type%% 
=%% 
document%% 
.%% 
Type%%  
;%%  !
}&& 	
}'' 
}(( ‡
cD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\Operator\OperatorListDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
Operator $
{		 
public

 

class

 
OperatorListDto

  
{ 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% )
)) *
]* +
public 
Guid 
Id 
{ 
get 
; 
} 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% /
)/ 0
]0 1
public 
string 
Username 
{  
get! $
;$ %
}& '
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% -
)- .
]. /
public 
UnitId 
UnitId 
{ 
get "
;" #
}$ %
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% ,
), -
]- .
public 
string 
Group 
{ 
get !
;! "
}# $
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 1
)1 2
]2 3
public 
string 

Assignment  
{! "
get# &
;& '
}( )
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% +
)+ ,
], -
public 
string 
Type 
{ 
get  
;  !
}" #
public 
OperatorListDto 
( 
OperatorDocument /
document0 8
)8 9
{ 	
Id   
=   
document   
.   
Identity   "
;  " #
Username!! 
=!! 
document!! 
.!!  
CoreAccount!!  +
.!!+ ,
Name!!, 0
;!!0 1
UnitId"" 
="" 
document"" 
."" 
UnitId"" $
;""$ %
Group## 
=## 
document## 
.## 
Group## "
;##" #

Assignment$$ 
=$$ 
document$$ !
.$$! "

Assignment$$" ,
;$$, -
Type%% 
=%% 
document%% 
.%% 
Type%%  
;%%  !
}&& 	
}'' 
}(( ü!
lD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\Order\ListWeavingOrderDocumentDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
Order !
{ 
public		 

class		 '
ListWeavingOrderDocumentDto		 ,
{

 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% )
)) *
]* +
public 
Guid 
Id 
{ 
get 
; 
} 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 2
)2 3
]3 4
public 
string 
OrderNumber !
{" #
get$ '
;' (
}) *
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 2
)2 3
]3 4
public 
DateTimeOffset 
DateOrdered )
{* +
get, /
;/ 0
}1 2
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 9
)9 :
]: ;
public 
string 
ConstructionNumber (
{) *
get+ .
;. /
}0 1
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 6
)6 7
]7 8
public 
Composition 
WarpComposition *
{+ ,
get- 0
;0 1
}2 3
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 6
)6 7
]7 8
public 
Composition 
WeftComposition *
{+ ,
get- 0
;0 1
}2 3
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 2
)2 3
]3 4
public 
UnitId 
UnitId 
{ 
get "
;" #
}$ %
[   	
JsonProperty  	 
(   
PropertyName   "
=  # $
$str  % 1
)  1 2
]  2 3
public!! 
string!! 

WarpOrigin!!  
{!!! "
get!!# &
;!!& '
}!!( )
[## 	
JsonProperty##	 
(## 
PropertyName## "
=### $
$str##% 1
)##1 2
]##2 3
public$$ 
string$$ 

WeftOrigin$$  
{$$! "
get$$# &
;$$& '
}$$( )
public&& '
ListWeavingOrderDocumentDto&& *
(&&* +
OrderDocument&&+ 8 
weavingOrderDocument&&9 M
,&&M N&
FabricConstructionDocument''+ E
fabricConstruction''F X
)''X Y
{(( 	
Id)) 
=))  
weavingOrderDocument)) %
.))% &
Identity))& .
;)). /
OrderNumber** 
=**  
weavingOrderDocument** .
.**. /
OrderNumber**/ :
;**: ;
DateOrdered++ 
=++  
weavingOrderDocument++ .
.++. /
DateOrdered++/ :
;++: ;
ConstructionNumber,, 
=,,  
fabricConstruction,,! 3
.,,3 4
ConstructionNumber,,4 F
;,,F G
WarpComposition-- 
=--  
weavingOrderDocument-- 2
.--2 3
WarpComposition--3 B
;--B C
WeftComposition.. 
=..  
weavingOrderDocument.. 2
...2 3
WeftComposition..3 B
;..B C
UnitId// 
=//  
weavingOrderDocument// )
.//) *
UnitId//* 0
;//0 1

WarpOrigin00 
=00  
weavingOrderDocument00 -
.00- .

WarpOrigin00. 8
;008 9

WeftOrigin11 
=11  
weavingOrderDocument11 -
.11- .

WeftOrigin11. 8
;118 9
}22 	
}33 
}44 ‡A
gD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\Order\OrderReportBySearchDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
Order !
{ 
public 

class "
OrderReportBySearchDto '
{ 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% )
)) *
]* +
public 
Guid 
Id 
{ 
get 
; 
private %
set& )
;) *
}+ ,
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 2
)2 3
]3 4
public 
string 
OrderNumber !
{" #
get$ '
;' (
private) 0
set1 4
;4 5
}6 7
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 2
)2 3
]3 4
public 
string 
OrderStatus !
{" #
get$ '
;' (
private) 0
set1 4
;4 5
}6 7
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% -
)- .
]. /
public 
Period 
Period 
{ 
get "
;" #
private$ +
set, /
;/ 0
}1 2
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 6
)6 7
]7 8
public 
string 
WeavingUnitName %
{& '
get( +
;+ ,
private- 4
set5 8
;8 9
}: ;
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 2
)2 3
]3 4
public   
DateTimeOffset   
DateOrdered   )
{  * +
get  , /
;  / 0
private  1 8
set  9 <
;  < =
}  > ?
["" 	
JsonProperty""	 
("" 
PropertyName"" "
=""# $
$str""% 6
)""6 7
]""7 8
public## 
Composition## 
WarpComposition## *
{##+ ,
get##- 0
;##0 1
private##2 9
set##: =
;##= >
}##? @
[%% 	
JsonProperty%%	 
(%% 
PropertyName%% "
=%%# $
$str%%% 6
)%%6 7
]%%7 8
public&& 
Composition&& 
WeftComposition&& *
{&&+ ,
get&&- 0
;&&0 1
private&&2 9
set&&: =
;&&= >
}&&? @
[(( 	
JsonProperty((	 
((( 
PropertyName(( "
=((# $
$str((% A
)((A B
]((B C
public)) +
ConstructionDocumentValueObject)) .&
FabricConstructionDocument))/ I
{))J K
get))L O
;))O P
private))Q X
set))Y \
;))\ ]
}))^ _
[++ 	
JsonProperty++	 
(++ 
PropertyName++ "
=++# $
$str++% 1
)++1 2
]++2 3
public,, 
string,, 

YarnNumber,,  
{,,! "
get,,# &
;,,& '
private,,( /
set,,0 3
;,,3 4
},,5 6
[.. 	
JsonProperty..	 
(.. 
PropertyName.. "
=..# $
$str..% 1
)..1 2
]..2 3
public// 
int// 

WholeGrade// 
{// 
get//  #
;//# $
private//% ,
set//- 0
;//0 1
}//2 3
[11 	
JsonProperty11	 
(11 
PropertyName11 "
=11# $
$str11% B
)11B C
]11C D
public22 2
&EstimatedProductionDocumentValueObject22 5'
EstimatedProductionDocument226 Q
{22R S
get22T W
;22W X
private22Y `
set22a d
;22d e
}22f g
public44 "
OrderReportBySearchDto44 %
(44% &
OrderDocument44& 3 
weavingOrderDocument444 H
,44H I&
FabricConstructionDocument44J d 
constructionDocument44e y
,44y z
List44{ 
<	44 Ä)
EstimatedProductionDocument
44Ä õ
>
44õ ú 
estimationDocument
44ù Ø
,
44Ø ∞
string
44± ∑

yarnNumber
44∏ ¬
,
44¬ √
string
44ƒ  
unit
44À œ
)
44œ –
{55 	
Id66 
=66  
weavingOrderDocument66 %
.66% &
Identity66& .
;66. /
OrderNumber77 
=77  
weavingOrderDocument77 .
.77. /
OrderNumber77/ :
;77: ;
OrderStatus88 
=88  
weavingOrderDocument88 .
.88. /
OrderStatus88/ :
;88: ;
Period99 
=99  
weavingOrderDocument99 )
.99) *
Period99* 0
;990 1

WholeGrade:: 
=::  
weavingOrderDocument:: -
.::- .

WholeGrade::. 8
;::8 9
WeavingUnitName;; 
=;; 
unit;; "
;;;" #
DateOrdered<< 
=<<  
weavingOrderDocument<< .
.<<. /
DateOrdered<</ :
;<<: ;
WarpComposition== 
===  
weavingOrderDocument== 2
.==2 3
WarpComposition==3 B
;==B C
WeftComposition>> 
=>>  
weavingOrderDocument>> 2
.>>2 3
WeftComposition>>3 B
;>>B C
var@@ 
construction@@ 
=@@ 
new@@ "+
ConstructionDocumentValueObject@@# B
(@@B C 
constructionDocument@@C W
.@@W X
Identity@@X `
,@@` a 
constructionDocumentAAD X
.AAX Y
ConstructionNumberAAY k
,AAk l 
constructionDocumentBBD X
.BBX Y
AmountOfWarpBBY e
,BBe f 
constructionDocumentCCD X
.CCX Y
AmountOfWeftCCY e
,CCe f 
constructionDocumentDDD X
.DDX Y
	TotalYarnDDY b
)DDb c
;DDc d&
FabricConstructionDocumentEE &
=EE' (
constructionEE) 5
;EE5 6

YarnNumberFF 
=FF 

yarnNumberFF #
;FF# $
foreachGG 
(GG 
varGG 
itemGG 
inGG  
estimationDocumentGG! 3
)GG3 4
{HH 
foreachII 
(II 
varII 
datumII "
inII# %
itemII& *
.II* +
EstimationProductsII+ =
)II= >
{JJ 
ifKK 
(KK 
datumKK 
.KK 
OrderDocumentKK +
.KK+ ,
DeserializeKK, 7
<KK7 8$
OrderDocumentValueObjectKK8 P
>KKP Q
(KKQ R
)KKR S
.KKS T
OrderNumberKKT _
.KK_ `
EqualsKK` f
(KKf g 
weavingOrderDocumentKKg {
.KK{ |
OrderNumber	KK| á
)
KKá à
)
KKà â
{LL 
varMM 
productGradeMM (
=MM) *
datumMM+ 0
.MM0 1
ProductGradeMM1 =
.MM= >
DeserializeMM> I
<MMI J
ProductGradeMMJ V
>MMV W
(MMW X
)MMX Y
;MMY Z'
EstimatedProductionDocumentNN 3
=NN4 5
newNN6 92
&EstimatedProductionDocumentValueObjectNN: `
(NN` a
productGradeNNa m
.NNm n
GradeANNn t
,NNt u
productGrade	NNv Ç
.
NNÇ É
GradeB
NNÉ â
,
NNâ ä
productGrade
NNã ó
.
NNó ò
GradeC
NNò û
,
NNû ü
productGrade
NN† ¨
.
NN¨ ≠
GradeD
NN≠ ≥
,
NN≥ ¥"
weavingOrderDocument
NNµ …
.
NN…  

WholeGrade
NN  ‘
)
NN‘ ’
;
NN’ ÷
}OO 
}PP 
}QQ 
}RR 	
}SS 
}TT ‘*
hD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\Order\WeavingOrderDocumentDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
Order !
{		 
public

 

class

 #
WeavingOrderDocumentDto

 (
{ 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% )
)) *
]* +
public 
Guid 
Id 
{ 
get 
; 
} 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 2
)2 3
]3 4
public 
string 
OrderNumber !
{" #
get$ '
;' (
}) *
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% A
)A B
]B C
public &
FabricConstructionDocument )&
FabricConstructionDocument* D
{E F
getG J
;J K
}L M
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 2
)2 3
]3 4
public 
DateTimeOffset 
DateOrdered )
{* +
get, /
;/ 0
}1 2
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 1
)1 2
]2 3
public 
string 

WarpOrigin  
{! "
get# &
;& '
}( )
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 1
)1 2
]2 3
public 
string 

WeftOrigin  
{! "
get# &
;& '
}( )
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 1
)1 2
]2 3
public 
int 

WholeGrade 
{ 
get  #
;# $
}% &
[!! 	
JsonProperty!!	 
(!! 
PropertyName!! "
=!!# $
$str!!% /
)!!/ 0
]!!0 1
public"" 
string"" 
YarnType"" 
{""  
get""! $
;""$ %
}""& '
[$$ 	
JsonProperty$$	 
($$ 
PropertyName$$ "
=$$# $
$str$$% -
)$$- .
]$$. /
public%% 
Period%% 
Period%% 
{%% 
get%% "
;%%" #
}%%$ %
['' 	
JsonProperty''	 
('' 
PropertyName'' "
=''# $
$str''% 6
)''6 7
]''7 8
public(( 
Composition(( 
WarpComposition(( *
{((+ ,
get((- 0
;((0 1
}((2 3
[** 	
JsonProperty**	 
(** 
PropertyName** "
=**# $
$str**% 6
)**6 7
]**7 8
public++ 
Composition++ 
WeftComposition++ *
{+++ ,
get++- 0
;++0 1
}++2 3
[-- 	
JsonProperty--	 
(-- 
PropertyName-- "
=--# $
$str--% 2
)--2 3
]--3 4
public.. 
UnitId.. 
UnitId.. 
{.. 
get.. "
;.." #
}..$ %
public00 #
WeavingOrderDocumentDto00 &
(00& '
OrderDocument00' 4 
weavingOrderDocument005 I
,00I J
UnitId11' -
unitId11. 4
,114 5&
FabricConstructionDocument22' A
fabricConstruction22B T
)22T U
{33 	
Id44 
=44  
weavingOrderDocument44 %
.44% &
Identity44& .
;44. /
OrderNumber55 
=55  
weavingOrderDocument55 .
.55. /
OrderNumber55/ :
;55: ;&
FabricConstructionDocument66 &
=66' (
fabricConstruction66) ;
;66; <
DateOrdered77 
=77  
weavingOrderDocument77 .
.77. /
DateOrdered77/ :
;77: ;

WarpOrigin88 
=88  
weavingOrderDocument88 -
.88- .

WarpOrigin88. 8
;888 9

WeftOrigin99 
=99  
weavingOrderDocument99 -
.99- .

WeftOrigin99. 8
;998 9

WholeGrade:: 
=::  
weavingOrderDocument:: -
.::- .

WholeGrade::. 8
;::8 9
YarnType;; 
=;;  
weavingOrderDocument;; +
.;;+ ,
YarnType;;, 4
;;;4 5
Period<< 
=<<  
weavingOrderDocument<< )
.<<) *
Period<<* 0
;<<0 1
WarpComposition== 
===  
weavingOrderDocument== 2
.==2 3
WarpComposition==3 B
;==B C
WeftComposition>> 
=>>  
weavingOrderDocument>> 2
.>>2 3
WeftComposition>>3 B
;>>B C
UnitId?? 
=??  
weavingOrderDocument?? )
.??) *
UnitId??* 0
;??0 1
}@@ 	
}AA 
}BB ≥
YD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\Shift\ShiftDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
Shift !
{ 
public 

class 
ShiftDto 
{ 
[		 	
JsonProperty			 
(		 
PropertyName		 "
=		# $
$str		% )
)		) *
]		* +
public

 
Guid

 
Id

 
{

 
get

 
;

 
}

 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% +
)+ ,
], -
public 
string 
Name 
{ 
get  
;  !
}" #
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 0
)0 1
]1 2
public 
TimeSpan 
	StartTime !
{" #
get$ '
;' (
}) *
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% .
). /
]/ 0
public 
TimeSpan 
EndTime 
{  !
get" %
;% &
}' (
public 
ShiftDto 
( 
ShiftDocument %
document& .
). /
{ 	
Id 
= 
document 
. 
Identity "
;" #
Name 
= 
document 
. 
Name  
;  !
	StartTime 
= 
document 
.  
	StartTime  )
;) *
EndTime 
= 
document 
. 
EndTime &
;& '
} 	
} 
} ì
nD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\WeavingSupplier\SupplierDocumentDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
WeavingSupplier +
{ 
public 

class 
SupplierDocumentDto $
{ 
[		 	
JsonProperty			 
(		 
PropertyName		 "
=		# $
$str		% )
)		) *
]		* +
public

 
Guid

 
Id

 
{

 
get

 
;

 
private

 %
set

& )
;

) *
}

+ ,
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% +
)+ ,
], -
public 
string 
Code 
{ 
get  
;  !
private" )
set* -
;- .
}/ 0
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% +
)+ ,
], -
public 
string 
Name 
{ 
get  
;  !
private" )
set* -
;- .
}/ 0
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 5
)5 6
]6 7
public 
string 
CoreSupplierId $
{% &
get' *
;* +
private, 3
set4 7
;7 8
}9 :
public 
SupplierDocumentDto "
(" ##
WeavingSupplierDocument# :#
weavingSupplierDocument; R
)R S
{ 	
Id 
= #
weavingSupplierDocument (
.( )
Identity) 1
;1 2
Code 
= #
weavingSupplierDocument *
.* +
Code+ /
;/ 0
Name 
= #
weavingSupplierDocument *
.* +
Name+ /
;/ 0
CoreSupplierId 
= #
weavingSupplierDocument 4
.4 5
CoreSupplierId5 C
;C D
} 	
} 
} µ
jD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\WeavingSupplier\SupplierListDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
WeavingSupplier +
{ 
public		 

class		 
SupplierListDto		  
{

 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% )
)) *
]* +
public 
Guid 
Id 
{ 
get 
; 
private %
set& )
;) *
}+ ,
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% +
)+ ,
], -
public 
string 
Code 
{ 
get  
;  !
private" )
set* -
;- .
}/ 0
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% +
)+ ,
], -
public 
string 
Name 
{ 
get  
;  !
private" )
set* -
;- .
}/ 0
public 
SupplierListDto 
( #
WeavingSupplierDocument 6#
weavingSupplierDocument7 N
)N O
{ 	
Id 
= #
weavingSupplierDocument (
.( )
Identity) 1
;1 2
Code 
= #
weavingSupplierDocument *
.* +
Code+ /
;/ 0
Name 
= #
weavingSupplierDocument *
.* +
Name+ /
;/ 0
} 	
} 
} ú	
YD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\WeavingUnitDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
{ 
public 

class 
WeavingUnitDto 
{		 
[

 	
JsonProperty

	 
(

 
PropertyName

 "
=

# $
$str

% )
)

) *
]

* +
public 
int 
Id 
{ 
get 
; 
set  
;  !
}" #
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% +
)+ ,
], -
public 
string 
Code 
{ 
get  
;  !
set" %
;% &
}' (
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% +
)+ ,
], -
public 
string 
Name 
{ 
get  
;  !
set" %
;% &
}' (
} 
} Ç
kD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\YarnNumber\YarnNumberDocumentDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 

YarnNumber &
{ 
public 

class !
YarnNumberDocumentDto &
{ 
[		 	
JsonProperty			 
(		 
PropertyName		 "
=		# $
$str		% )
)		) *
]		* +
public

 
Guid

 
Id

 
{

 
get

 
;

 
private

 %
set

& )
;

) *
}

+ ,
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% +
)+ ,
], -
public 
string 
Code 
{ 
get  
;  !
private" )
set* -
;- .
}/ 0
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% -
)- .
]. /
public 
int 
Number 
{ 
get 
;  
private! (
set) ,
;, -
}. /
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% /
)/ 0
]0 1
public 
string 
RingType 
{  
get! $
;$ %
private& -
set. 1
;1 2
}3 4
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% 2
)2 3
]3 4
public 
string 
Description !
{" #
get$ '
;' (
private) 0
set1 4
;4 5
}6 7
public !
YarnNumberDocumentDto $
($ %
YarnNumberDocument% 7
ringDocument8 D
)D E
{ 	
Id 
= 
ringDocument 
. 
Identity &
;& '
Code 
= 
ringDocument 
.  
Code  $
;$ %
Number 
= 
ringDocument !
.! "
Number" (
;( )
RingType 
= 
ringDocument #
.# $
RingType$ ,
;, -
Description 
= 
ringDocument &
.& '
Description' 2
;2 3
} 	
}   
}!! ‰
gD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\YarnNumber\YarnNumberListDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 

YarnNumber &
{ 
public		 

class		 
YarnNumberListDto		 "
{

 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% )
)) *
]* +
public 
Guid 
Id 
{ 
get 
; 
} 
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% +
)+ ,
], -
public 
string 
Code 
{ 
get  
;  !
}" #
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% -
)- .
]. /
public 
int 
Number 
{ 
get 
;  
}! "
[ 	
JsonProperty	 
( 
PropertyName "
=# $
$str% /
)/ 0
]0 1
public 
string 
RingType 
{  
get! $
;$ %
}& '
public 
YarnNumberListDto  
(  !
YarnNumberDocument! 3
ringDocument4 @
)@ A
{ 	
Id 
= 
ringDocument 
. 
Identity &
;& '
Code 
= 
ringDocument 
.  
Code  $
;$ %
Number 
= 
ringDocument !
.! "
Number" (
;( )
RingType 
= 
ringDocument #
.# $
RingType$ ,
;, -
} 	
} 
} ≠
_D:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\Yarn\YarnDocumentDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
Yarn  
{ 
public 

class 
YarnDocumentDto  
{		 
[

 	
JsonProperty

	 
(

 
propertyName

 "
:

" #
$str

$ (
)

( )
]

) *
public 
Guid 
Id 
{ 
get 
; 
} 
[ 	
JsonProperty	 
( 
propertyName "
:" #
$str$ *
)* +
]+ ,
public 
string 
Code 
{ 
get  
;  !
}" #
[ 	
JsonProperty	 
( 
propertyName "
:" #
$str$ *
)* +
]+ ,
public 
string 
Name 
{ 
get  
;  !
}" #
[ 	
JsonProperty	 
( 
propertyName "
:" #
$str$ *
)* +
]+ ,
public 
string 
Tags 
{ 
get  
;  !
}" #
[ 	
JsonProperty	 
( 
propertyName "
:" #
$str$ 4
)4 5
]5 6
public #
MaterialTypeValueObject & 
MaterialTypeDocument' ;
{< =
get> A
;A B
}C D
[ 	
JsonProperty	 
( 
propertyName "
:" #
$str$ 2
)2 3
]3 4
public !
YarnNumberValueObject $
YarnNumberDocument% 7
{8 9
get: =
;= >
}? @
public 
YarnDocumentDto 
( 
YarnDocument +
yarn, 0
,0 1#
MaterialTypeValueObject 6
materialType7 C
,C D!
YarnNumberValueObject 4

yarnNumber5 ?
)? @
{ 	
Id   
=   
yarn   
.   
Identity   
;   
Code!! 
=!! 
yarn!! 
.!! 
Code!! 
;!! 
Name"" 
="" 
yarn"" 
."" 
Name"" 
;"" 
Tags## 
=## 
yarn## 
.## 
Tags## 
;##  
MaterialTypeDocument$$  
=$$! "
materialType$$# /
;$$/ 0
YarnNumberDocument%% 
=%%  

yarnNumber%%! +
;%%+ ,
}&& 	
}'' 
}(( ï
cD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Dtos\Yarn\YarnDocumentListDto.cs
	namespace 	
Manufactures
 
. 
Dtos 
. 
Yarn  
{ 
public 

class 
YarnDocumentListDto $
{		 
[

 	
JsonProperty

	 
(

 
propertyName

 "
:

" #
$str

$ (
)

( )
]

) *
public 
Guid 
Id 
{ 
get 
; 
} 
[ 	
JsonProperty	 
( 
propertyName "
:" #
$str$ *
)* +
]+ ,
public 
string 
Code 
{ 
get  
;  !
}" #
[ 	
JsonProperty	 
( 
propertyName "
:" #
$str$ *
)* +
]+ ,
public 
string 
Name 
{ 
get  
;  !
}" #
[ 	
JsonProperty	 
( 
propertyName "
:" #
$str$ 4
)4 5
]5 6
public #
MaterialTypeValueObject &
MaterialType' 3
{4 5
get6 9
;9 :
}; <
[ 	
JsonProperty	 
( 
propertyName "
:" #
$str$ 2
)2 3
]3 4
public !
YarnNumberValueObject $

YarnNumber% /
{0 1
get2 5
;5 6
}7 8
public 
YarnDocumentListDto "
(" #
YarnDocument# /
document0 8
)8 9
{ 	
Id 
= 
document 
. 
Identity "
;" #
Code 
= 
document 
. 
Code  
;  !
Name 
= 
document 
. 
Name  
;  !
} 	
public   
YarnDocumentListDto   "
(  " #
YarnDocument  # /
document  0 8
,  8 9#
MaterialTypeValueObject!!# :
materialType!!; G
,!!G H!
YarnNumberValueObject""# 8

yarnNumber""9 C
)""C D
{## 	
Id$$ 
=$$ 
document$$ 
.$$ 
Identity$$ "
;$$" #
Code%% 
=%% 
document%% 
.%% 
Code%%  
;%%  !
Name&& 
=&& 
document&& 
.&& 
Name&&  
;&&  !
MaterialType'' 
='' 
materialType'' '
;''' (

YarnNumber(( 
=(( 

yarnNumber(( #
;((# $
})) 	
}** 
}++ ¥
tD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\EventHandlers\OnManufactureOrderCreatedHandler.cs
	namespace 	
Manufactures
 
. 
Domain 
. 
Events $
{ 
public 

class ,
 OnManufactureOrderCreatedHandler 1
:2 3
IDomainEventHandler4 G
<G H
OnEntityCreatedH W
<W X
ManufactureOrderX h
>h i
>i j
{		 
public

 
Task

 
Handle

 
(

 
OnEntityCreated

 *
<

* +
ManufactureOrder

+ ;
>

; <
notification

= I
,

I J
CancellationToken

K \
cancellationToken

] n
)

n o
{ 	
return 
Task 
. 
CompletedTask %
;% &
} 	
} 
} —
sD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\EventHandlers\OnManufactureOrderPlacedHandler.cs
	namespace 	
Manufactures
 
. 
Domain 
. 
Events $
{ 
public 

class +
OnManufactureOrderPlacedHandler 0
:1 2$
IManufactureEventHandler3 K
<K L$
OnManufactureOrderPlacedL d
>d e
{ 
public 
Task 
Handle 
( $
OnManufactureOrderPlaced 3
notification4 @
,@ A
CancellationTokenB S
cancellationTokenT e
)e f
{		 	
return

 
Task

 
.

 
CompletedTask

 %
;

% &
} 	
} 
} ¥
tD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\EventHandlers\OnManufactureOrderUpdatedHandler.cs
	namespace 	
Manufactures
 
. 
Domain 
. 
Events $
{ 
public 

class ,
 OnManufactureOrderUpdatedHandler 1
:2 3
IDomainEventHandler4 G
<G H
OnEntityUpdatedH W
<W X
ManufactureOrderX h
>h i
>i j
{		 
public

 
Task

 
Handle

 
(

 
OnEntityUpdated

 *
<

* +
ManufactureOrder

+ ;
>

; <
notification

= I
,

I J
CancellationToken

K \
cancellationToken

] n
)

n o
{ 	
return 
Task 
. 
CompletedTask %
;% &
} 	
} 
} ç

WD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\ExtensionMetadata.cs
	namespace 	
Manufactures
 
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
StyleSheets' 2
=>3 5
new6 9

StyleSheet: D
[D E
]E F
{G H
}I J
;J K
public 
IEnumerable 
< 
Script !
>! "
Scripts# *
=>+ -
new. 1
Script2 8
[8 9
]9 :
{; <
}= >
;> ?
public 
IEnumerable 
< 
MenuItem #
># $
	MenuItems% .
{ 	
get 
{ 
return 
new 
MenuItem #
[# $
]$ %
{ 
new 
MenuItem  
(  !
$str! /
,/ 0
$str1 ?
,? @
$numA D
)D E
} 
; 
} 
} 	
} 
} °‘
uD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\Helpers\PdfTemplates\OrderProductionPDFTemplate.cs
	namespace 	
Manufactures
 
. 
Helpers 
. 
PdfTemplates +
{ 
public 

class &
OrderProductionPDFTemplate +
{ 
const 
int 
MARGIN 
= 
$num 
; 
const 
string 
TITLE 
= 
$str E
;E F
const 
string 
UNIT 
= 
$str "
;" #
const 
string 
SPLITTER 
= 
$str  %
;% &
const 
string 

EMPTY_SPOT 
=  !
$str" %
;% &
const 
string 
NUMBER 
= 
$str #
;# $
const 
string 
DATE_ORDERED !
=" #
$str$ 1
;1 2
const 
string 
CONSTRUCTION !
=" #
$str$ 0
;0 1
const 
string 
YARN_NUMBER  
=! "
$str# /
;/ 0
const 
string 
BLENDED_WARP_POLY &
=' (
$str) /
+0 1
$str2 6
+7 8
$str9 B
;B C
const 
string 
BLENDED_WARP_COTTON (
=) *
$str+ 1
+2 3
$str4 8
+9 :
$str; F
;F G
const 
string 
BLENDED_WARP_OTHERS (
=) *
$str+ 1
+2 3
$str4 8
+9 :
$str; G
;G H
const 
string 
BLENDED_WEFT_POLY &
=' (
$str) 0
+1 2
$str3 7
+8 9
$str: C
;C D
const 
string 
BLENDED_WEFT_COTTON (
=) *
$str+ 2
+3 4
$str5 9
+: ;
$str< G
;G H
const   
string   
BLENDED_WEFT_OTHERS   (
=  ) *
$str  + 2
+  3 4
$str  5 9
+  : ;
$str  < H
;  H I
const!! 
string!! 
ESTIMATED_GRADE_A!! &
=!!' (
$str!!) 2
;!!2 3
const"" 
string"" 
ESTIMATED_GRADE_B"" &
=""' (
$str"") 2
;""2 3
const## 
string## 
ESTIMATED_GRADE_C## &
=##' (
$str##) 2
;##2 3
const$$ 
string$$ 
ESTIMATED_GRADE_D$$ &
=$$' (
$str$$) 2
;$$2 3
const%% 
string%% 
WHOLE_GRADE%%  
=%%! "
$str%%# .
;%%. /
const&& 
string&& $
FABRIC_CONSTRUCTION_WARP&& -
=&&. /
$str&&0 ;
+&&< =
$str&&> B
+&&C D
$str&&E K
;&&K L
const'' 
string'' $
FABRIC_CONSTRUCTION_WEFT'' -
=''. /
$str''0 ;
+''< =
$str''> B
+''C D
$str''E L
;''L M
const(( 
string(( *
FABRIC_CONSTRUCTION_TOTAL_YARN(( 3
=((4 5
$str((6 =
+((> ?
$str((@ D
+((E F
$str((G R
;((R S
const)) 
string)) 
KNOWING)) 
=)) 
$str)) ,
;)), -
const** 
string** 
MADE_BY** 
=** 
$str** -
;**- .
const++ 
string++ 

BLANK_SPOT++ 
=++  !
$str++" J
;++J K
const,, 
string,, 
HEAD_OF_DIVISION,, %
=,,& '
$str,,( 7
;,,7 8
const-- 
string-- "
MAINTENANCE_PRODUCTION-- +
=--, -
$str--. E
;--E F
const.. 
string.. 
PPIC_WEAVING.. !
=.." #
$str..$ 2
;..2 3
private44 
static44 
readonly44 
Font44  $

title_font44% /
=440 1
FontFactory442 =
.44= >
GetFont44> E
(44E F
BaseFont44F N
.44N O
	HELVETICA44O X
,44X Y
BaseFont44Z b
.44b c
CP125044c i
,44i j
BaseFont44k s
.44s t
NOT_EMBEDDED	44t Ä
,
44Ä Å
$num
44Ç Ñ
)
44Ñ Ö
;
44Ö Ü
private55 
static55 
readonly55 
Font55  $
normal_font55% 0
=551 2
FontFactory553 >
.55> ?
GetFont55? F
(55F G
BaseFont55G O
.55O P
	HELVETICA55P Y
,55Y Z
BaseFont55[ c
.55c d
CP125055d j
,55j k
BaseFont55l t
.55t u
NOT_EMBEDDED	55u Å
,
55Å Ç
$num
55É Ñ
)
55Ñ Ö
;
55Ö Ü
private66 
static66 
readonly66 
Font66  $
	bold_font66% .
=66/ 0
FontFactory661 <
.66< =
GetFont66= D
(66D E
BaseFont66E M
.66M N
HELVETICA_BOLD66N \
,66\ ]
BaseFont66^ f
.66f g
CP125066g m
,66m n
BaseFont66o w
.66w x
NOT_EMBEDDED	66x Ñ
,
66Ñ Ö
$num
66Ü á
)
66á à
;
66à â
private<< 
readonly<< 
	PdfPTable<< "
Title<<# (
;<<( )
private== 
readonly== 
	PdfPTable== "
Header==# )
;==) *
private>> 
readonly>> 
	PdfPTable>> "
Body>># '
;>>' (
private?? 
readonly?? 
	PdfPTable?? "

BodyFooter??# -
;??- .
private@@ 
readonly@@ 
	PdfPTable@@ "
Footer@@# )
;@@) *
publicDD &
OrderProductionPDFTemplateDD )
(DD) *
ListDD* .
<DD. /"
OrderReportBySearchDtoDD/ E
>DDE F
reportModelDDG R
)DDR S
{EE 	
ListNN 
<NN 
stringNN 
>NN 

bodyColumnNN #
=NN$ %
newNN& )
ListNN* .
<NN. /
stringNN/ 5
>NN5 6
{NN7 8
DATE_ORDEREDNN9 E
,NNE F
CONSTRUCTIONNNG S
,NNS T
YARN_NUMBERNNU `
,NN` a
BLENDED_WARP_POLYNNb s
,NNs t 
BLENDED_WARP_COTTON	NNu à
,
NNà â!
BLENDED_WARP_OTHERS
NNä ù
,
NNù û
BLENDED_WEFT_POLY
NNü ∞
,
NN∞ ±!
BLENDED_WEFT_COTTON
NN≤ ≈
,
NN≈ ∆!
BLENDED_WEFT_OTHERS
NN« ⁄
,
NN⁄ €
ESTIMATED_GRADE_A
NN‹ Ì
,
NNÌ Ó
ESTIMATED_GRADE_B
NNÔ Ä
,
NNÄ Å
ESTIMATED_GRADE_C
NNÇ ì
,
NNì î
ESTIMATED_GRADE_D
NNï ¶
,
NN¶ ß
WHOLE_GRADE
NN® ≥
,
NN≥ ¥&
FABRIC_CONSTRUCTION_WARP
NNµ Õ
,
NNÕ Œ&
FABRIC_CONSTRUCTION_WEFT
NNœ Á
,
NNÁ Ë,
FABRIC_CONSTRUCTION_TOTAL_YARN
NNÈ á
}
NNà â
;
NNâ ä
ListPP 
<PP 
ListPP 
<PP 
stringPP 
>PP 
>PP 
bodyDataPP '
=PP( )
newPP* -
ListPP. 2
<PP2 3
ListPP3 7
<PP7 8
stringPP8 >
>PP> ?
>PP? @
{QQ 
reportModelRR 
.RR 
SelectRR "
(RR" #
xRR# $
=>RR% '
xRR( )
.RR) *
DateOrderedRR* 5
.RR5 6
DateRR6 :
.RR: ;
ToStringRR; C
(RRC D
$strRRD P
)RRP Q
)RRQ R
.RRR S
ToListRRS Y
(RRY Z
)RRZ [
,RR[ \
reportModelSS 
.SS 
SelectSS "
(SS" #
xSS# $
=>SS% '
xSS( )
.SS) *&
FabricConstructionDocumentSS* D
.SSD E
ConstructionNumberSSE W
)SSW X
.SSX Y
ToListSSY _
(SS_ `
)SS` a
,SSa b
reportModelTT 
.TT 
SelectTT "
(TT" #
xTT# $
=>TT% '
xTT( )
.TT) *

YarnNumberTT* 4
)TT4 5
.TT5 6
ToListTT6 <
(TT< =
)TT= >
,TT> ?
reportModelUU 
.UU 
SelectUU "
(UU" #
xUU# $
=>UU% '
xUU( )
.UU) *
WarpCompositionUU* 9
.UU9 :
CompositionOfPolyUU: K
.UUK L
ToStringUUL T
(UUT U
)UUU V
)UUV W
.UUW X
ToListUUX ^
(UU^ _
)UU_ `
,UU` a
reportModelVV 
.VV 
SelectVV "
(VV" #
xVV# $
=>VV% '
xVV( )
.VV) *
WarpCompositionVV* 9
.VV9 :
CompositionOfCottonVV: M
.VVM N
ToStringVVN V
(VVV W
)VVW X
)VVX Y
.VVY Z
ToListVVZ `
(VV` a
)VVa b
,VVb c
reportModelWW 
.WW 
SelectWW "
(WW" #
xWW# $
=>WW% '
xWW( )
.WW) *
WarpCompositionWW* 9
.WW9 :
OtherCompositionWW: J
.WWJ K
ToStringWWK S
(WWS T
)WWT U
)WWU V
.WWV W
ToListWWW ]
(WW] ^
)WW^ _
,WW_ `
reportModelXX 
.XX 
SelectXX "
(XX" #
xXX# $
=>XX% '
xXX( )
.XX) *
WeftCompositionXX* 9
.XX9 :
CompositionOfPolyXX: K
.XXK L
ToStringXXL T
(XXT U
)XXU V
)XXV W
.XXW X
ToListXXX ^
(XX^ _
)XX_ `
,XX` a
reportModelYY 
.YY 
SelectYY "
(YY" #
xYY# $
=>YY% '
xYY( )
.YY) *
WeftCompositionYY* 9
.YY9 :
CompositionOfCottonYY: M
.YYM N
ToStringYYN V
(YYV W
)YYW X
)YYX Y
.YYY Z
ToListYYZ `
(YY` a
)YYa b
,YYb c
reportModelZZ 
.ZZ 
SelectZZ "
(ZZ" #
xZZ# $
=>ZZ% '
xZZ( )
.ZZ) *
WeftCompositionZZ* 9
.ZZ9 :
OtherCompositionZZ: J
.ZZJ K
ToStringZZK S
(ZZS T
)ZZT U
)ZZU V
.ZZV W
ToListZZW ]
(ZZ] ^
)ZZ^ _
,ZZ_ `
reportModel[[ 
.[[ 
Select[[ "
([[" #
x[[# $
=>[[% '
x[[( )
.[[) *'
EstimatedProductionDocument[[* E
.[[E F
GradeA[[F L
.[[L M
ToString[[M U
([[U V
)[[V W
)[[W X
.[[X Y
ToList[[Y _
([[_ `
)[[` a
,[[a b
reportModel\\ 
.\\ 
Select\\ "
(\\" #
x\\# $
=>\\% '
x\\( )
.\\) *'
EstimatedProductionDocument\\* E
.\\E F
GradeB\\F L
.\\L M
ToString\\M U
(\\U V
)\\V W
)\\W X
.\\X Y
ToList\\Y _
(\\_ `
)\\` a
,\\a b
reportModel]] 
.]] 
Select]] "
(]]" #
x]]# $
=>]]% '
x]]( )
.]]) *'
EstimatedProductionDocument]]* E
.]]E F
GradeC]]F L
.]]L M
ToString]]M U
(]]U V
)]]V W
)]]W X
.]]X Y
ToList]]Y _
(]]_ `
)]]` a
,]]a b
reportModel^^ 
.^^ 
Select^^ "
(^^" #
x^^# $
=>^^% '
x^^( )
.^^) *'
EstimatedProductionDocument^^* E
.^^E F
GradeD^^F L
.^^L M
ToString^^M U
(^^U V
)^^V W
)^^W X
.^^X Y
ToList^^Y _
(^^_ `
)^^` a
,^^a b
reportModel__ 
.__ 
Select__ "
(__" #
x__# $
=>__% '
x__( )
.__) *'
EstimatedProductionDocument__* E
.__E F

WholeGrade__F P
.__P Q
ToString__Q Y
(__Y Z
)__Z [
)__[ \
.__\ ]
ToList__] c
(__c d
)__d e
,__e f
reportModel`` 
.`` 
Select`` "
(``" #
x``# $
=>``% '
x``( )
.``) *&
FabricConstructionDocument``* D
.``D E
AmountOfWarp``E Q
.``Q R
ToString``R Z
(``Z [
)``[ \
)``\ ]
.``] ^
ToList``^ d
(``d e
)``e f
,``f g
reportModelaa 
.aa 
Selectaa "
(aa" #
xaa# $
=>aa% '
xaa( )
.aa) *&
FabricConstructionDocumentaa* D
.aaD E
AmountOfWeftaaE Q
.aaQ R
ToStringaaR Z
(aaZ [
)aa[ \
)aa\ ]
.aa] ^
ToListaa^ d
(aad e
)aae f
,aaf g
reportModelbb 
.bb 
Selectbb "
(bb" #
xbb# $
=>bb% '
xbb( )
.bb) *&
FabricConstructionDocumentbb* D
.bbD E
	TotalYarnbbE N
.bbN O
ToStringbbO W
(bbW X
)bbX Y
)bbY Z
.bbZ [
ToListbb[ a
(bba b
)bbb c
}cc 
;cc 
stringee 
unitNameee 
=ee 
reportModelee )
.ee) *
Selectee* 0
(ee0 1
uee1 2
=>ee3 5
uee6 7
.ee7 8
WeavingUnitNameee8 G
)eeG H
.eeH I
FirstOrDefaulteeI W
(eeW X
)eeX Y
;eeY Z
thisii 
.ii 
Titleii 
=ii 
GetTitleii !
(ii! "
)ii" #
;ii# $
thisjj 
.jj 
Headerjj 
=jj 
	GetHeaderjj #
(jj# $
unitNamejj$ ,
)jj, -
;jj- .
thiskk 
.kk 
Bodykk 
=kk 
GetBodykk 
(kk  

bodyColumnkk  *
,kk* +
bodyDatakk, 4
)kk4 5
;kk5 6
thisll 
.ll 

BodyFooterll 
=ll 
thisll "
.ll" #
GetBodyFooterll# 0
(ll0 1
)ll1 2
;ll2 3
thismm 
.mm 
Footermm 
=mm 
	GetFootermm #
(mm# $
)mm$ %
;mm% &
}nn 	
privatepp 
	PdfPTablepp 
GetTitlepp "
(pp" #
)pp# $
{qq 	
	PdfPTablerr 
titlerr 
=rr 
newrr !
	PdfPTablerr" +
(rr+ ,
$numrr, -
)rr- .
{ss 
WidthPercentagett 
=tt  !
$numtt" %
}uu 
;uu 
PdfPCellvv 
	cellTitlevv 
=vv  
newvv! $
PdfPCellvv% -
(vv- .
)vv. /
{ww 
Borderxx 
=xx 
	Rectanglexx "
.xx" #
	NO_BORDERxx# ,
,xx, -
HorizontalAlignmentyy #
=yy$ %
Elementyy& -
.yy- .
ALIGN_CENTERyy. :
,yy: ;
PaddingBottomzz 
=zz 
$numzz  #
}{{ 
;{{ 
	cellTitle|| 
.|| 
Phrase|| 
=|| 
new|| "
Phrase||# )
(||) *
TITLE||* /
,||/ 0

title_font||1 ;
)||; <
;||< =
title}} 
.}} 
AddCell}} 
(}} 
	cellTitle}} #
)}}# $
;}}$ %
return 
title 
; 
}
ÄÄ 	
private
ÇÇ 
	PdfPTable
ÇÇ 
	GetHeader
ÇÇ #
(
ÇÇ# $
string
ÇÇ$ *

headerData
ÇÇ+ 5
)
ÇÇ5 6
{
ÉÉ 	
	PdfPTable
ÑÑ 
header
ÑÑ 
=
ÑÑ 
new
ÑÑ "
	PdfPTable
ÑÑ# ,
(
ÑÑ, -
$num
ÑÑ- .
)
ÑÑ. /
;
ÑÑ/ 0
float
ÖÖ 
[
ÖÖ 
]
ÖÖ 
headerTableWidths
ÖÖ %
=
ÖÖ& '
new
ÖÖ( +
float
ÖÖ, 1
[
ÖÖ1 2
]
ÖÖ2 3
{
ÖÖ4 5
$num
ÖÖ6 8
,
ÖÖ8 9
$num
ÖÖ: <
,
ÖÖ< =
$num
ÖÖ> @
,
ÖÖ@ A
$num
ÖÖB E
}
ÖÖF G
;
ÖÖG H
header
ÜÜ 
.
ÜÜ 
	SetWidths
ÜÜ 
(
ÜÜ 
headerTableWidths
ÜÜ .
)
ÜÜ. /
;
ÜÜ/ 0
header
áá 
.
áá 
WidthPercentage
áá "
=
áá# $
$num
áá% (
;
áá( )
PdfPCell
àà 
cellHeader1
àà  
=
àà! "
new
àà# &
PdfPCell
àà' /
(
àà/ 0
)
àà0 1
{
ââ 
Border
ää 
=
ää 
	Rectangle
ää "
.
ää" #
	NO_BORDER
ää# ,
,
ää, -!
HorizontalAlignment
ãã #
=
ãã$ %
Element
ãã& -
.
ãã- .

ALIGN_LEFT
ãã. 8
,
ãã8 9
PaddingBottom
åå 
=
åå 
$num
åå  #
}
çç 
;
çç 
cellHeader1
éé 
.
éé 
Phrase
éé 
=
éé  
new
éé! $
Phrase
éé% +
(
éé+ ,
UNIT
éé, 0
,
éé0 1
normal_font
éé2 =
)
éé= >
;
éé> ?
header
èè 
.
èè 
AddCell
èè 
(
èè 
cellHeader1
èè &
)
èè& '
;
èè' (
PdfPCell
ëë 
cellHeader2
ëë  
=
ëë! "
new
ëë# &
PdfPCell
ëë' /
(
ëë/ 0
)
ëë0 1
{
íí 
Border
ìì 
=
ìì 
	Rectangle
ìì "
.
ìì" #
	NO_BORDER
ìì# ,
,
ìì, -!
HorizontalAlignment
îî #
=
îî$ %
Element
îî& -
.
îî- .

ALIGN_LEFT
îî. 8
,
îî8 9
PaddingBottom
ïï 
=
ïï 
$num
ïï  #
}
ññ 
;
ññ 
cellHeader2
óó 
.
óó 
Phrase
óó 
=
óó  
new
óó! $
Phrase
óó% +
(
óó+ ,
SPLITTER
óó, 4
,
óó4 5
normal_font
óó6 A
)
óóA B
;
óóB C
header
òò 
.
òò 
AddCell
òò 
(
òò 
cellHeader2
òò &
)
òò& '
;
òò' (
PdfPCell
öö 
cellHeader3
öö  
=
öö! "
new
öö# &
PdfPCell
öö' /
(
öö/ 0
)
öö0 1
{
õõ 
Border
úú 
=
úú 
	Rectangle
úú "
.
úú" #
	NO_BORDER
úú# ,
,
úú, -!
HorizontalAlignment
ùù #
=
ùù$ %
Element
ùù& -
.
ùù- .

ALIGN_LEFT
ùù. 8
,
ùù8 9
PaddingBottom
ûû 
=
ûû 
$num
ûû  #
}
üü 
;
üü 
cellHeader3
†† 
.
†† 
Phrase
†† 
=
††  
new
††! $
Phrase
††% +
(
††+ ,

headerData
††, 6
,
††6 7
normal_font
††8 C
)
††C D
;
††D E
header
°° 
.
°° 
AddCell
°° 
(
°° 
cellHeader3
°° &
)
°°& '
;
°°' (
PdfPCell
££ 
cellHeader4
££  
=
££! "
new
££# &
PdfPCell
££' /
(
££/ 0
)
££0 1
{
§§ 
Border
•• 
=
•• 
	Rectangle
•• "
.
••" #
	NO_BORDER
••# ,
,
••, -
PaddingBottom
¶¶ 
=
¶¶ 
$num
¶¶  #
}
ßß 
;
ßß 
cellHeader4
®® 
.
®® 
Phrase
®® 
=
®®  
new
®®! $
Phrase
®®% +
(
®®+ ,

EMPTY_SPOT
®®, 6
,
®®6 7
normal_font
®®8 C
)
®®C D
;
®®D E
header
©© 
.
©© 
AddCell
©© 
(
©© 
cellHeader4
©© &
)
©©& '
;
©©' (
return
´´ 
header
´´ 
;
´´ 
}
¨¨ 	
private
ÆÆ 
	PdfPTable
ÆÆ 
GetBody
ÆÆ !
(
ÆÆ! "
List
ÆÆ" &
<
ÆÆ& '
string
ÆÆ' -
>
ÆÆ- .

bodyColumn
ÆÆ/ 9
,
ÆÆ9 :
List
ÆÆ; ?
<
ÆÆ? @
List
ÆÆ@ D
<
ÆÆD E
string
ÆÆE K
>
ÆÆK L
>
ÆÆL M
bodyData
ÆÆN V
)
ÆÆV W
{
ØØ 	
	PdfPTable
∞∞ 
	bodyTable
∞∞ 
=
∞∞  !
new
∞∞" %
	PdfPTable
∞∞& /
(
∞∞/ 0
$num
∞∞0 2
)
∞∞2 3
;
∞∞3 4
float
±± 
[
±± 
]
±± 
bodyTableWidths
±± #
=
±±$ %
new
±±& )
float
±±* /
[
±±/ 0
]
±±0 1
{
±±2 3
$num
±±4 6
,
±±6 7
$num
±±8 :
,
±±: ;
$num
±±< ?
,
±±? @
$num
±±A C
,
±±C D
$num
±±E G
,
±±G H
$num
±±I K
,
±±K L
$num
±±M O
,
±±O P
$num
±±Q S
,
±±S T
$num
±±U W
,
±±W X
$num
±±Y [
,
±±[ \
$num
±±] _
,
±±_ `
$num
±±a c
,
±±c d
$num
±±e g
,
±±g h
$num
±±i k
,
±±k l
$num
±±m o
,
±±o p
$num
±±q s
,
±±s t
$num
±±u w
,
±±w x
$num
±±y {
}
±±| }
;
±±} ~
	bodyTable
≤≤ 
.
≤≤ 
	SetWidths
≤≤ 
(
≤≤  
bodyTableWidths
≤≤  /
)
≤≤/ 0
;
≤≤0 1
	bodyTable
≥≥ 
.
≥≥ 
WidthPercentage
≥≥ %
=
≥≥& '
$num
≥≥( +
;
≥≥+ ,
PdfPCell
µµ 
bodyCell
µµ 
=
µµ 
new
µµ  #
PdfPCell
µµ$ ,
(
µµ, -
)
µµ- .
{
µµ/ 0
Border
µµ1 7
=
µµ8 9
	Rectangle
µµ: C
.
µµC D
BOX
µµD G
,
µµG H!
HorizontalAlignment
µµI \
=
µµ] ^
Element
µµ_ f
.
µµf g
ALIGN_CENTER
µµg s
,
µµs t 
VerticalAlignmentµµu Ü
=µµá à
Elementµµâ ê
.µµê ë
ALIGN_MIDDLEµµë ù
}µµû ü
;µµü †
PdfPCell
∂∂ 
	emptyCell
∂∂ 
=
∂∂  
new
∂∂! $
PdfPCell
∂∂% -
(
∂∂- .
)
∂∂. /
{
∂∂0 1
Border
∂∂2 8
=
∂∂9 :
	Rectangle
∂∂; D
.
∂∂D E
BOX
∂∂E H
,
∂∂H I
FixedHeight
∂∂J U
=
∂∂V W
$num
∂∂X [
}
∂∂\ ]
;
∂∂] ^
	emptyCell
∑∑ 
.
∑∑ 
Phrase
∑∑ 
=
∑∑ 
new
∑∑ "
Phrase
∑∑# )
(
∑∑) *
string
∑∑* 0
.
∑∑0 1
Empty
∑∑1 6
,
∑∑6 7
normal_font
∑∑8 C
)
∑∑C D
;
∑∑D E
bodyCell
ππ 
.
ππ 
Phrase
ππ 
=
ππ 
new
ππ !
Phrase
ππ" (
(
ππ( )
$str
ππ) -
,
ππ- .
	bold_font
ππ/ 8
)
ππ8 9
;
ππ9 :
	bodyTable
∫∫ 
.
∫∫ 
AddCell
∫∫ 
(
∫∫ 
bodyCell
∫∫ &
)
∫∫& '
;
∫∫' (
foreach
ºº 
(
ºº 
var
ºº 
column
ºº 
in
ºº  "

bodyColumn
ºº# -
)
ºº- .
{
ΩΩ 
bodyCell
ææ 
.
ææ 
Phrase
ææ 
=
ææ  !
new
ææ" %
Phrase
ææ& ,
(
ææ, -
column
ææ- 3
,
ææ3 4
	bold_font
ææ5 >
)
ææ> ?
;
ææ? @
	bodyTable
øø 
.
øø 
AddCell
øø !
(
øø! "
bodyCell
øø" *
)
øø* +
;
øø+ ,
}
¿¿ 
for
¬¬ 
(
¬¬ 
int
¬¬ 
rowNo
¬¬ 
=
¬¬ 
$num
¬¬ 
;
¬¬ 
rowNo
¬¬  %
<
¬¬& '
bodyData
¬¬( 0
.
¬¬0 1
FirstOrDefault
¬¬1 ?
(
¬¬? @
)
¬¬@ A
.
¬¬A B
Count
¬¬B G
;
¬¬G H
rowNo
¬¬I N
++
¬¬N P
)
¬¬P Q
{
√√ 
bodyCell
ƒƒ 
.
ƒƒ 
Phrase
ƒƒ 
=
ƒƒ  !
new
ƒƒ" %
Phrase
ƒƒ& ,
(
ƒƒ, -
(
ƒƒ- .
rowNo
ƒƒ. 3
+
ƒƒ4 5
$num
ƒƒ6 7
)
ƒƒ7 8
.
ƒƒ8 9
ToString
ƒƒ9 A
(
ƒƒA B
$str
ƒƒB G
,
ƒƒG H
CultureInfo
ƒƒI T
.
ƒƒT U
InvariantCulture
ƒƒU e
)
ƒƒe f
,
ƒƒf g
normal_font
ƒƒh s
)
ƒƒs t
;
ƒƒt u
	bodyTable
≈≈ 
.
≈≈ 
AddCell
≈≈ !
(
≈≈! "
bodyCell
≈≈" *
)
≈≈* +
;
≈≈+ ,
for
«« 
(
«« 
int
«« 
colNo
«« 
=
««  
$num
««! "
;
««" #
colNo
««$ )
<
««* +
bodyData
««, 4
.
««4 5
Count
««5 :
;
««: ;
colNo
««< A
++
««A C
)
««C D
{
»» 
bodyCell
…… 
.
…… 
Phrase
…… #
=
……$ %
new
……& )
Phrase
……* 0
(
……0 1
bodyData
……1 9
[
……9 :
colNo
……: ?
]
……? @
[
……@ A
rowNo
……A F
]
……F G
,
……G H
normal_font
……I T
)
……T U
;
……U V
	bodyTable
   
.
   
AddCell
   %
(
  % &
bodyCell
  & .
)
  . /
;
  / 0
}
ÀÀ 
}
ÃÃ 
return
ŒŒ 
	bodyTable
ŒŒ 
;
ŒŒ 
}
œœ 	
private
—— 
	PdfPTable
—— 
GetBodyFooter
—— '
(
——' (
)
——( )
{
““ 	
	PdfPTable
”” 
bodyFooterTable
”” %
=
””& '
new
””( +
	PdfPTable
””, 5
(
””5 6
$num
””6 7
)
””7 8
;
””8 9
bodyFooterTable
‘‘ 
.
‘‘ 
	SetWidths
‘‘ %
(
‘‘% &
new
‘‘& )
float
‘‘* /
[
‘‘/ 0
]
‘‘0 1
{
‘‘2 3
$num
‘‘4 6
,
‘‘6 7
$num
‘‘8 :
}
‘‘; <
)
‘‘< =
;
‘‘= >
bodyFooterTable
’’ 
.
’’ 
WidthPercentage
’’ +
=
’’, -
$num
’’. 1
;
’’1 2
	PdfPTable
◊◊  
subBodyFooterTable
◊◊ (
=
◊◊) *
new
◊◊+ .
	PdfPTable
◊◊/ 8
(
◊◊8 9
$num
◊◊9 :
)
◊◊: ;
;
◊◊; < 
subBodyFooterTable
ÿÿ 
.
ÿÿ 
WidthPercentage
ÿÿ .
=
ÿÿ/ 0
$num
ÿÿ1 4
;
ÿÿ4 5 
subBodyFooterTable
ŸŸ 
.
ŸŸ !
HorizontalAlignment
ŸŸ 2
=
ŸŸ3 4
Element
ŸŸ5 <
.
ŸŸ< =
ALIGN_CENTER
ŸŸ= I
;
ŸŸI J
PdfPCell
€€ 
bodyFooterCell
€€ #
=
€€$ %
new
€€& )
PdfPCell
€€* 2
(
€€2 3
)
€€3 4
{
€€5 6
Border
€€7 =
=
€€> ?
	Rectangle
€€@ I
.
€€I J
	NO_BORDER
€€J S
,
€€S T!
HorizontalAlignment
€€U h
=
€€i j
Element
€€k r
.
€€r s
ALIGN_CENTER
€€s 
}€€Ä Å
;€€Å Ç
bodyFooterCell
‹‹ 
.
‹‹ 

PaddingTop
‹‹ %
=
‹‹& '
$num
‹‹( +
;
‹‹+ ,
PdfPCell
›› 
subBodyFooterCell
›› &
=
››' (
new
››) ,
PdfPCell
››- 5
(
››5 6
)
››6 7
{
››8 9
Border
››: @
=
››A B
	Rectangle
››C L
.
››L M
	NO_BORDER
››M V
,
››V W!
HorizontalAlignment
››X k
=
››l m
Element
››n u
.
››u v
ALIGN_CENTER››v Ç
}››É Ñ
;››Ñ Ö
PdfPCell
ﬁﬁ 
	emptyCell
ﬁﬁ 
=
ﬁﬁ  
new
ﬁﬁ! $
PdfPCell
ﬁﬁ% -
(
ﬁﬁ- .
)
ﬁﬁ. /
{
ﬁﬁ0 1
Border
ﬁﬁ2 8
=
ﬁﬁ9 :
	Rectangle
ﬁﬁ; D
.
ﬁﬁD E
	NO_BORDER
ﬁﬁE N
,
ﬁﬁN O
FixedHeight
ﬁﬁP [
=
ﬁﬁ\ ]
$num
ﬁﬁ^ a
}
ﬁﬁb c
;
ﬁﬁc d
	emptyCell
ﬂﬂ 
.
ﬂﬂ 
Phrase
ﬂﬂ 
=
ﬂﬂ 
new
ﬂﬂ "
Phrase
ﬂﬂ# )
(
ﬂﬂ) *
string
ﬂﬂ* 0
.
ﬂﬂ0 1
Empty
ﬂﬂ1 6
,
ﬂﬂ6 7
normal_font
ﬂﬂ8 C
)
ﬂﬂC D
;
ﬂﬂD E
subBodyFooterCell
·· 
.
·· 
Phrase
·· $
=
··% &
new
··' *
Phrase
··+ 1
(
··1 2
KNOWING
··2 9
,
··9 :
normal_font
··; F
)
··F G
;
··G H 
subBodyFooterTable
‚‚ 
.
‚‚ 
AddCell
‚‚ &
(
‚‚& '
subBodyFooterCell
‚‚' 8
)
‚‚8 9
;
‚‚9 :
bodyFooterCell
„„ 
.
„„ 

AddElement
„„ %
(
„„% & 
subBodyFooterTable
„„& 8
)
„„8 9
;
„„9 :
bodyFooterTable
ÂÂ 
.
ÂÂ 
AddCell
ÂÂ #
(
ÂÂ# $
bodyFooterCell
ÂÂ$ 2
)
ÂÂ2 3
;
ÂÂ3 4
bodyFooterCell
ÁÁ 
=
ÁÁ 
new
ÁÁ  
PdfPCell
ÁÁ! )
(
ÁÁ) *
)
ÁÁ* +
{
ÁÁ, -
Border
ÁÁ. 4
=
ÁÁ5 6
	Rectangle
ÁÁ7 @
.
ÁÁ@ A
	NO_BORDER
ÁÁA J
,
ÁÁJ K!
HorizontalAlignment
ÁÁL _
=
ÁÁ` a
Element
ÁÁb i
.
ÁÁi j
ALIGN_CENTER
ÁÁj v
}
ÁÁw x
;
ÁÁx y 
subBodyFooterTable
ËË 
=
ËË  
new
ËË! $
	PdfPTable
ËË% .
(
ËË. /
$num
ËË/ 0
)
ËË0 1
;
ËË1 2 
subBodyFooterTable
ÈÈ 
.
ÈÈ 
WidthPercentage
ÈÈ .
=
ÈÈ/ 0
$num
ÈÈ1 4
;
ÈÈ4 5 
subBodyFooterTable
ÍÍ 
.
ÍÍ !
HorizontalAlignment
ÍÍ 2
=
ÍÍ3 4
Element
ÍÍ5 <
.
ÍÍ< =
ALIGN_CENTER
ÍÍ= I
;
ÍÍI J
subBodyFooterCell
ÏÏ 
.
ÏÏ 
Phrase
ÏÏ $
=
ÏÏ% &
new
ÏÏ' *
Phrase
ÏÏ+ 1
(
ÏÏ1 2
MADE_BY
ÏÏ2 9
,
ÏÏ9 :
normal_font
ÏÏ; F
)
ÏÏF G
;
ÏÏG H 
subBodyFooterTable
ÌÌ 
.
ÌÌ 
AddCell
ÌÌ &
(
ÌÌ& '
subBodyFooterCell
ÌÌ' 8
)
ÌÌ8 9
;
ÌÌ9 :
bodyFooterCell
ÓÓ 
.
ÓÓ 

AddElement
ÓÓ %
(
ÓÓ% & 
subBodyFooterTable
ÓÓ& 8
)
ÓÓ8 9
;
ÓÓ9 :
bodyFooterTable
 
.
 
AddCell
 #
(
# $
bodyFooterCell
$ 2
)
2 3
;
3 4
return
ÚÚ 
bodyFooterTable
ÚÚ "
;
ÚÚ" #
}
ÛÛ 	
private
ıı 
	PdfPTable
ıı 
	GetFooter
ıı #
(
ıı# $
)
ıı$ %
{
ˆˆ 	
	PdfPTable
˜˜ 
footerTable
˜˜ !
=
˜˜" #
new
˜˜$ '
	PdfPTable
˜˜( 1
(
˜˜1 2
$num
˜˜2 3
)
˜˜3 4
;
˜˜4 5
footerTable
¯¯ 
.
¯¯ 
	SetWidths
¯¯ !
(
¯¯! "
new
¯¯" %
float
¯¯& +
[
¯¯+ ,
]
¯¯, -
{
¯¯. /
$num
¯¯0 2
,
¯¯2 3
$num
¯¯4 6
,
¯¯6 7
$num
¯¯8 :
}
¯¯; <
)
¯¯< =
;
¯¯= >
footerTable
˘˘ 
.
˘˘ 
WidthPercentage
˘˘ '
=
˘˘( )
$num
˘˘* -
;
˘˘- .
	PdfPTable
˚˚ 
subFooterTable
˚˚ $
=
˚˚% &
new
˚˚' *
	PdfPTable
˚˚+ 4
(
˚˚4 5
$num
˚˚5 6
)
˚˚6 7
;
˚˚7 8
subFooterTable
¸¸ 
.
¸¸ 
WidthPercentage
¸¸ *
=
¸¸+ ,
$num
¸¸- 0
;
¸¸0 1
subFooterTable
˝˝ 
.
˝˝ !
HorizontalAlignment
˝˝ .
=
˝˝/ 0
Element
˝˝1 8
.
˝˝8 9
ALIGN_CENTER
˝˝9 E
;
˝˝E F
PdfPCell
ˇˇ 

footerCell
ˇˇ 
=
ˇˇ  !
new
ˇˇ" %
PdfPCell
ˇˇ& .
(
ˇˇ. /
)
ˇˇ/ 0
{
ˇˇ1 2
Border
ˇˇ3 9
=
ˇˇ: ;
	Rectangle
ˇˇ< E
.
ˇˇE F
	NO_BORDER
ˇˇF O
,
ˇˇO P!
HorizontalAlignment
ˇˇQ d
=
ˇˇe f
Element
ˇˇg n
.
ˇˇn o
ALIGN_CENTER
ˇˇo {
}
ˇˇ| }
;
ˇˇ} ~
PdfPCell
ÄÄ 
subFooterCell
ÄÄ "
=
ÄÄ# $
new
ÄÄ% (
PdfPCell
ÄÄ) 1
(
ÄÄ1 2
)
ÄÄ2 3
{
ÄÄ4 5
Border
ÄÄ6 <
=
ÄÄ= >
	Rectangle
ÄÄ? H
.
ÄÄH I
	NO_BORDER
ÄÄI R
,
ÄÄR S!
HorizontalAlignment
ÄÄT g
=
ÄÄh i
Element
ÄÄj q
.
ÄÄq r
ALIGN_CENTER
ÄÄr ~
}ÄÄ Ä
;ÄÄÄ Å
PdfPCell
ÅÅ 
	emptyCell
ÅÅ 
=
ÅÅ  
new
ÅÅ! $
PdfPCell
ÅÅ% -
(
ÅÅ- .
)
ÅÅ. /
{
ÅÅ0 1
Border
ÅÅ2 8
=
ÅÅ9 :
	Rectangle
ÅÅ; D
.
ÅÅD E
	NO_BORDER
ÅÅE N
,
ÅÅN O
FixedHeight
ÅÅP [
=
ÅÅ\ ]
$num
ÅÅ^ a
}
ÅÅb c
;
ÅÅc d
	emptyCell
ÇÇ 
.
ÇÇ 
Phrase
ÇÇ 
=
ÇÇ 
new
ÇÇ "
Phrase
ÇÇ# )
(
ÇÇ) *
string
ÇÇ* 0
.
ÇÇ0 1
Empty
ÇÇ1 6
,
ÇÇ6 7
normal_font
ÇÇ8 C
)
ÇÇC D
;
ÇÇD E
subFooterTable
ÑÑ 
.
ÑÑ 
AddCell
ÑÑ "
(
ÑÑ" #
	emptyCell
ÑÑ# ,
)
ÑÑ, -
;
ÑÑ- .
subFooterTable
ÖÖ 
.
ÖÖ 
AddCell
ÖÖ "
(
ÖÖ" #
	emptyCell
ÖÖ# ,
)
ÖÖ, -
;
ÖÖ- .
subFooterCell
ÜÜ 
.
ÜÜ 
Phrase
ÜÜ  
=
ÜÜ! "
new
ÜÜ# &
Phrase
ÜÜ' -
(
ÜÜ- .

BLANK_SPOT
ÜÜ. 8
,
ÜÜ8 9
normal_font
ÜÜ: E
)
ÜÜE F
;
ÜÜF G
subFooterTable
áá 
.
áá 
AddCell
áá "
(
áá" #
subFooterCell
áá# 0
)
áá0 1
;
áá1 2
subFooterCell
àà 
.
àà 
Phrase
àà  
=
àà! "
new
àà# &
Phrase
àà' -
(
àà- .
HEAD_OF_DIVISION
àà. >
,
àà> ?
normal_font
àà@ K
)
ààK L
;
ààL M
subFooterTable
ââ 
.
ââ 
AddCell
ââ "
(
ââ" #
subFooterCell
ââ# 0
)
ââ0 1
;
ââ1 2

footerCell
ää 
.
ää 

AddElement
ää !
(
ää! "
subFooterTable
ää" 0
)
ää0 1
;
ää1 2
footerTable
åå 
.
åå 
AddCell
åå 
(
åå  

footerCell
åå  *
)
åå* +
;
åå+ ,

footerCell
éé 
=
éé 
new
éé 
PdfPCell
éé %
(
éé% &
)
éé& '
{
éé( )
Border
éé* 0
=
éé1 2
	Rectangle
éé3 <
.
éé< =
	NO_BORDER
éé= F
,
ééF G!
HorizontalAlignment
ééH [
=
éé\ ]
Element
éé^ e
.
éée f
ALIGN_CENTER
ééf r
}
éés t
;
éét u
subFooterTable
èè 
=
èè 
new
èè  
	PdfPTable
èè! *
(
èè* +
$num
èè+ ,
)
èè, -
;
èè- .
subFooterTable
êê 
.
êê 
WidthPercentage
êê *
=
êê+ ,
$num
êê- 0
;
êê0 1
subFooterTable
ëë 
.
ëë !
HorizontalAlignment
ëë .
=
ëë/ 0
Element
ëë1 8
.
ëë8 9
ALIGN_CENTER
ëë9 E
;
ëëE F
subFooterTable
ìì 
.
ìì 
AddCell
ìì "
(
ìì" #
	emptyCell
ìì# ,
)
ìì, -
;
ìì- .
subFooterTable
îî 
.
îî 
AddCell
îî "
(
îî" #
	emptyCell
îî# ,
)
îî, -
;
îî- .
subFooterCell
ïï 
.
ïï 
Phrase
ïï  
=
ïï! "
new
ïï# &
Phrase
ïï' -
(
ïï- .

BLANK_SPOT
ïï. 8
,
ïï8 9
normal_font
ïï: E
)
ïïE F
;
ïïF G
subFooterTable
ññ 
.
ññ 
AddCell
ññ "
(
ññ" #
subFooterCell
ññ# 0
)
ññ0 1
;
ññ1 2
subFooterCell
óó 
.
óó 
Phrase
óó  
=
óó! "
new
óó# &
Phrase
óó' -
(
óó- .$
MAINTENANCE_PRODUCTION
óó. D
,
óóD E
normal_font
óóF Q
)
óóQ R
;
óóR S
subFooterTable
òò 
.
òò 
AddCell
òò "
(
òò" #
subFooterCell
òò# 0
)
òò0 1
;
òò1 2

footerCell
ôô 
.
ôô 

AddElement
ôô !
(
ôô! "
subFooterTable
ôô" 0
)
ôô0 1
;
ôô1 2
footerTable
õõ 
.
õõ 
AddCell
õõ 
(
õõ  

footerCell
õõ  *
)
õõ* +
;
õõ+ ,

footerCell
ùù 
=
ùù 
new
ùù 
PdfPCell
ùù %
(
ùù% &
)
ùù& '
{
ùù( )
Border
ùù* 0
=
ùù1 2
	Rectangle
ùù3 <
.
ùù< =
	NO_BORDER
ùù= F
,
ùùF G!
HorizontalAlignment
ùùH [
=
ùù\ ]
Element
ùù^ e
.
ùùe f
ALIGN_CENTER
ùùf r
}
ùùs t
;
ùùt u
subFooterTable
ûû 
=
ûû 
new
ûû  
	PdfPTable
ûû! *
(
ûû* +
$num
ûû+ ,
)
ûû, -
;
ûû- .
subFooterTable
üü 
.
üü 
WidthPercentage
üü *
=
üü+ ,
$num
üü- 0
;
üü0 1
subFooterTable
†† 
.
†† !
HorizontalAlignment
†† .
=
††/ 0
Element
††1 8
.
††8 9
ALIGN_CENTER
††9 E
;
††E F
subFooterTable
¢¢ 
.
¢¢ 
AddCell
¢¢ "
(
¢¢" #
	emptyCell
¢¢# ,
)
¢¢, -
;
¢¢- .
subFooterTable
££ 
.
££ 
AddCell
££ "
(
££" #
	emptyCell
££# ,
)
££, -
;
££- .
subFooterCell
§§ 
.
§§ 
Phrase
§§  
=
§§! "
new
§§# &
Phrase
§§' -
(
§§- .

BLANK_SPOT
§§. 8
,
§§8 9
normal_font
§§: E
)
§§E F
;
§§F G
subFooterTable
•• 
.
•• 
AddCell
•• "
(
••" #
subFooterCell
••# 0
)
••0 1
;
••1 2
subFooterCell
¶¶ 
.
¶¶ 
Phrase
¶¶  
=
¶¶! "
new
¶¶# &
Phrase
¶¶' -
(
¶¶- .
PPIC_WEAVING
¶¶. :
,
¶¶: ;
normal_font
¶¶< G
)
¶¶G H
;
¶¶H I
subFooterTable
ßß 
.
ßß 
AddCell
ßß "
(
ßß" #
subFooterCell
ßß# 0
)
ßß0 1
;
ßß1 2

footerCell
®® 
.
®® 

AddElement
®® !
(
®®! "
subFooterTable
®®" 0
)
®®0 1
;
®®1 2
footerTable
™™ 
.
™™ 
AddCell
™™ 
(
™™  

footerCell
™™  *
)
™™* +
;
™™+ ,
return
¨¨ 
footerTable
¨¨ 
;
¨¨ 
}
≠≠ 	
public
ØØ 
MemoryStream
ØØ !
GeneratePdfTemplate
ØØ /
(
ØØ/ 0
)
ØØ0 1
{
∞∞ 	
Document
≤≤ 
document
≤≤ 
=
≤≤ 
new
≤≤  #
Document
≤≤$ ,
(
≤≤, -
PageSize
≤≤- 5
.
≤≤5 6
A4
≤≤6 8
.
≤≤8 9
Rotate
≤≤9 ?
(
≤≤? @
)
≤≤@ A
,
≤≤A B
MARGIN
≤≤C I
,
≤≤I J
MARGIN
≤≤K Q
,
≤≤Q R
MARGIN
≤≤S Y
,
≤≤Y Z
MARGIN
≤≤[ a
)
≤≤a b
;
≤≤b c
MemoryStream
≥≥ 
stream
≥≥ 
=
≥≥  !
new
≥≥" %
MemoryStream
≥≥& 2
(
≥≥2 3
)
≥≥3 4
;
≥≥4 5
	PdfWriter
¥¥ 
writer
¥¥ 
=
¥¥ 
	PdfWriter
¥¥ (
.
¥¥( )
GetInstance
¥¥) 4
(
¥¥4 5
document
¥¥5 =
,
¥¥= >
stream
¥¥? E
)
¥¥E F
;
¥¥F G
document
µµ 
.
µµ 
Open
µµ 
(
µµ 
)
µµ 
;
µµ 
document
∑∑ 
.
∑∑ 
Add
∑∑ 
(
∑∑ 
Title
∑∑ 
)
∑∑ 
;
∑∑  
document
∏∏ 
.
∏∏ 
Add
∏∏ 
(
∏∏ 
Header
∏∏ 
)
∏∏  
;
∏∏  !
document
ππ 
.
ππ 
Add
ππ 
(
ππ 
Body
ππ 
)
ππ 
;
ππ 
document
∫∫ 
.
∫∫ 
Add
∫∫ 
(
∫∫ 

BodyFooter
∫∫ #
)
∫∫# $
;
∫∫$ %
document
ªª 
.
ªª 
Add
ªª 
(
ªª 
Footer
ªª 
)
ªª  
;
ªª  !
document
ΩΩ 
.
ΩΩ 
Close
ΩΩ 
(
ΩΩ 
)
ΩΩ 
;
ΩΩ 
byte
øø 
[
øø 
]
øø 
byteInfo
øø 
=
øø 
stream
øø $
.
øø$ %
ToArray
øø% ,
(
øø, -
)
øø- .
;
øø. /
stream
¿¿ 
.
¿¿ 
Write
¿¿ 
(
¿¿ 
byteInfo
¿¿ !
,
¿¿! "
$num
¿¿# $
,
¿¿$ %
byteInfo
¿¿& .
.
¿¿. /
Length
¿¿/ 5
)
¿¿5 6
;
¿¿6 7
stream
¡¡ 
.
¡¡ 
Position
¡¡ 
=
¡¡ 
$num
¡¡ 
;
¡¡  
return
√√ 
stream
√√ 
;
√√ 
}
ƒƒ 	
}
≈≈ 
}∆∆ ë
WD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\ServicesRegistrar.cs
	namespace 	
Manufactures
 
{ 
public 

class 
ServicesRegistrar "
:# $$
IConfigureServicesAction% =
{ 
public		 
int		 
Priority		 
=>		 
$num		 #
;		# $
public 
void 
Execute 
( 
IServiceCollection .
services/ 7
,7 8
IServiceProvider9 I
spJ L
)L M
{ 	
} 	
} 
} ˙
lD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\ViewModels\Manufacture\CreateViewModel.cs
	namespace		 	
Manufactures		
 
.		 

ViewModels		 !
.		! "
Manufacture		" -
{

 
public 

class 
CreateViewModel  
{ 
[ 	
Display	 
( 
Name 
= 
$str 
) 
]  
[ 	
Required	 
] 
public 
DateTimeOffset 
	OrderDate '
{( )
get* -
;- .
set/ 2
;2 3
}4 5
[ 	
Display	 
( 
Name 
= 
$str 
) 
]  
[ 	
Required	 
] 
public 
int 
UnitDepartmentId #
{$ %
get& )
;) *
set+ .
;. /
}0 1
[ 	
Display	 
( 
Name 
= 
$str $
)$ %
]% &
[ 	
Required	 
] 
public 
List 
< 
string 
> 
	YarnCodes %
{& '
get( +
;+ ,
set- 0
;0 1
}2 3
[ 	
Display	 
( 
Name 
= 
$str !
)! "
]" #
[ 	
Required	 
] 
public 
List 
< 
float 
> 
Blended "
{# $
get% (
;( )
set* -
;- .
}/ 0
[ 	
Display	 
( 
Name 
= 
$str !
)! "
]" #
[ 	
Required	 
] 
public 
int 
	MachineId 
{ 
get "
;" #
set$ '
;' (
}) *
[$$ 	
Display$$	 
($$ 
Name$$ 
=$$ 
$str$$ 
)$$ 
]$$  
[%% 	
Required%%	 
]%% 
public&& 
string&& 
UserId&& 
{&& 
get&& "
;&&" #
set&&$ '
;&&' (
}&&) *
[(( 	
Display((	 
((( 
Name(( 
=(( 
$str(( &
)((& '
]((' (
public)) 
GoodsCompositionId)) !
CompositionId))" /
{))0 1
get))2 5
;))5 6
set))7 :
;)): ;
}))< =
}** 
}++ ∏
rD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\ViewModels\Manufacture\CreateViewModelMapper.cs
	namespace 	
Manufactures
 
. 

ViewModels !
.! "
Manufacture" -
{		 
public

 

class

 !
CreateViewModelMapper

 &
{ 
public 
ManufactureOrder 
Map  #
(# $
CreateViewModel$ 3
createViewModel4 C
,C D
stringE K
currentUserL W
)W X
{ 	
return 
new 
ManufactureOrder '
(' (
id( *
:* +
Guid, 0
.0 1
NewGuid1 8
(8 9
)9 :
,: ;
	orderDate 
: 
createViewModel .
.. /
	OrderDate/ 8
.8 9
Date9 =
,= >
unitId 
: 
new 
UnitDepartmentId  0
(0 1
createViewModel1 @
.@ A
UnitDepartmentIdA Q
)Q R
,R S
	yarnCodes 
: 
new "
	YarnCodes# ,
(, -
createViewModel- <
.< =
	YarnCodes= F
)F G
,G H
compositionId !
:! "
createViewModel# 2
.2 3
CompositionId3 @
,@ A
blended 
: 
new  
Blended! (
(( )
createViewModel) 8
.8 9
Blended9 @
)@ A
,A B
	machineId 
: 
new " 
MachineIdValueObject# 7
(7 8
createViewModel8 G
.G H
	MachineIdH Q
)Q R
,R S
userId 
: 
currentUser '
)' (
;( )
} 	
} 
} Ê
kD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\ViewModels\Manufacture\IndexViewModel.cs
	namespace 	
Manufactures
 
. 

ViewModels !
.! "
Manufacture" -
{ 
public 

class 
IndexViewModel 
{ 
} 
}		 ´
rD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures\ViewModels\Manufacture\IndexViewModelFactory.cs
	namespace 	
Manufactures
 
. 

ViewModels !
.! "
Manufacture" -
{ 
public 

class !
IndexViewModelFactory &
{		 
public

 
IndexViewModel

 
Create

 $
(

$ %
IStorage

% -
storage

. 5
)

5 6
{ 	
return 
new 
IndexViewModel %
(% &
)& '
{ 
} 
; 
} 	
} 
} 