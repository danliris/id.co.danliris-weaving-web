�"
sD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Infrastructure.Data.EntityFrameworkCore\AggregateRepostory.cs
	namespace
Infrastructure
 
.
Data
.
EntityFrameworkCore
{ 
public 

abstract 
class 
AggregateRepostory ,
<, -

TAggregate- 7
,7 8

TReadModel9 C
>C D
:E F
RepositoryBaseG U
<U V

TReadModelV `
>` a
,a b 
IAggregateRepositoryc w
<w x

TAggregate	x �
,
� �

TReadModel
� �
>
� �
where 

TAggregate 
: 

<( )

TAggregate) 3
,3 4

TReadModel5 ?
>? @
where 

TReadModel 
: 

{ 
public 

IQueryable 
< 

TReadModel $
>$ %
Query& +
=>, .
dbSet/ 4
;4 5
public 
List 
< 

TAggregate 
> 
Find  $
($ %

IQueryable% /
</ 0

TReadModel0 :
>: ;

readModels< F
)F G
{ 	
return 

readModels 
. 
Select $
($ %
o% &
=>' )
Map* -
(- .
o. /
)/ 0
)0 1
.1 2
ToList2 8
(8 9
)9 :
;: ;
} 	
public 
List 
< 

TAggregate 
> 
Find  $
($ %

Expression% /
</ 0
Func0 4
<4 5

TReadModel5 ?
,? @
boolA E
>E F
>F G
	conditionH Q
)Q R
{ 	
return 
Query 
. 
Where 
( 
	condition (
)( )
.) *
Select* 0
(0 1
o1 2
=>3 5
Map6 9
(9 :
o: ;
); <
)< =
.= >
ToList> D
(D E
)E F
;F G
} 	
	protected 
abstract 

TAggregate %
Map& )
() *

TReadModel* 4
	readModel5 >
)> ?
;? @
public!! 
virtual!! 
Task!! 
Update!! "
(!!" #

TAggregate!!# -
	aggregate!!. 7
)!!7 8
{"" 	
if## 
(## 
	aggregate## 
.## 
IsTransient## %
(##% &
)##& '
)##' (
dbSet$$ 
.$$ 
Add$$ 
($$ 
	aggregate$$ #
.$$# $
GetReadModel$$$ 0
($$0 1
)$$1 2
)$$2 3
;$$3 4
else%% 
if%% 
(%% 
	aggregate%% 
.%% 

IsModified%% )
(%%) *
)%%* +
)%%+ ,
dbSet&& 
.&& 
Update&& 
(&& 
	aggregate&& &
.&&& '
GetReadModel&&' 3
(&&3 4
)&&4 5
)&&5 6
;&&6 7
else'' 
if'' 
('' 
	aggregate'' 
.'' 
	IsRemoved'' (
(''( )
)'') *
)''* +
{(( 
var)) 
	readModel)) 
=)) 
	aggregate))  )
.))) *
GetReadModel))* 6
())6 7
)))7 8
;))8 9
if** 
(** 
	readModel** 
is**  
ISoftDelete**! ,
)**, -
{++ 
	readModel,, 
.,, 
Deleted,, %
=,,& '
true,,( ,
;,,, -
dbSet-- 
.-- 
Update--  
(--  !
	readModel--! *
)--* +
;--+ ,
}.. 
else// 
dbSet00 
.00 
Remove00  
(00  !
	readModel00! *
)00* +
;00+ ,
}11 
return33 
Task33 
.33 

;33% &
}44 	
}55 
}66 