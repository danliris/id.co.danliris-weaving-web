‡
eD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Domain.Events\IManufactureEvent.cs
	namespace 	
Manufactures
 
. 
Domain 
. 
Events $
{ 
public 

	interface 
IManufactureEvent &
:' (
IDomainEvent) 5
{ 
} 
} Õ
lD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Domain.Events\IManufactureEventHandler.cs
	namespace 	
Manufactures
 
. 
Domain 
. 
Events $
{ 
public 

	interface $
IManufactureEventHandler -
<- .
TEvent. 4
>4 5
:6 7
IDomainEventHandler8 K
<K L
TEventL R
>R S
whereT Y
TEventZ `
:a b
IManufactureEventc t
{ 
} 
} ³
]D:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Domain.Events\OnAddBeam.cs
	namespace 	
Manufactures
 
. 
Domain 
. 
Events $
{ 
public 

class 
	OnAddBeam 
: 
IManufactureEvent .
{ 
public 
Guid 
Id 
{ 
get 
; 
} 
public		 
	OnAddBeam		 
(		 
Guid		 
id		  
)		  !
{

 	
Id 
= 
id 
; 
} 	
} 
} Ý
kD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Domain.Events\OnAddDailyOperationLoom.cs
	namespace 	
Manufactures
 
. 
Domain 
. 
Events $
{ 
public 

class #
OnAddDailyOperationLoom (
:) *
IManufactureEvent+ <
{ 
public		 
Guid		 
Id		 
{		 
get		 
;		 
}		 
public #
OnAddDailyOperationLoom &
(& '
Guid' +
id, .
). /
{ 	
Id 
= 
id 
; 
} 	
} 
} Ñ
gD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Domain.Events\OnAddEnginePlanning.cs
	namespace 	
Manufactures
 
. 
Domain 
. 
Events $
{ 
public 

class 
OnAddEnginePlanning $
:% &
IManufactureEvent' 8
{ 
public		 
Guid		 
Id		 
{		 
get		 
;		 
}		 
public 
OnAddEnginePlanning "
(" #
Guid# '
id( *
)* +
{ 	
Id 
= 
id 
; 
} 	
} 
} Å
cD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Domain.Events\OnAddEstimation.cs
	namespace 	
Manufactures
 
. 
Domain 
. 
Events $
{ 
public 

class 
OnAddEstimation  
:! "
IManufactureEvent# 4
{ 
public		 
Guid		 
Id		 
{		 
get		 
;		 
}		 
public 
OnAddEstimation 
( 
Guid #
id$ &
)& '
{ 	
Id 
= 
id 
; 
} 	
} 
} È
dD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Domain.Events\OnAddMachineType.cs
	namespace 	
Manufactures
 
. 
Domain 
. 
Events $
{ 
public 

class 
OnAddMachineType !
:" #
IManufactureEvent$ 5
{ 
public 
Guid 
Id 
{ 
get 
; 
} 
public		 
OnAddMachineType		 
(		  
Guid		  $
id		% '
)		' (
{

 	
Id 
= 
id 
; 
} 	
} 
} þ
nD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Domain.Events\OnFabricConstructionPlaced.cs
	namespace 	
Manufactures
 
. 
Domain 
. 
Events $
{ 
public 

class &
OnFabricConstructionPlaced +
:, -
IManufactureEvent. ?
{ 
public		 &
OnFabricConstructionPlaced		 )
(		) *
Guid		* .
identity		/ 7
)		7 8
{

 	
Identity 
= 
identity 
;  
} 	
public 
Guid 
Identity 
{ 
get "
;" #
}$ %
} 
} ô
lD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Domain.Events\OnManufactureOrderPlaced.cs
	namespace 	
Manufactures
 
. 
Domain 
. 
Events $
{ 
public 

class $
OnManufactureOrderPlaced )
:* +
IManufactureEvent, =
{ 
public $
OnManufactureOrderPlaced '
(' (
Guid( ,
orderId- 4
)4 5
{ 	
OrderID		 
=		 
orderId		 
;		 
}

 	
public 
Guid 
OrderID 
{ 
get !
;! "
}# $
} 
} Ñ
gD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Domain.Events\OnMaterialTypePlace.cs
	namespace 	
Manufactures
 
. 
Domain 
. 
Events $
{ 
public 

class 
OnMaterialTypePlace $
:% &
IManufactureEvent' 8
{ 
public 
OnMaterialTypePlace "
(" #
Guid# '
id( *
)* +
{ 	
Id		 
=		 
id		 
;		 
}

 	
public 
Guid 
Id 
{ 
get 
; 
} 
} 
} è
hD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Domain.Events\OnWeavingOrderPlaced.cs
	namespace 	
Manufactures
 
. 
Domain 
. 
Events $
{ 
public 

class  
OnWeavingOrderPlaced %
:& '
IManufactureEvent( 9
{ 
public  
OnWeavingOrderPlaced #
(# $
Guid$ (
orderId) 0
)0 1
{ 	
OrderId		 
=		 
orderId		 
;		 
}

 	
public 
Guid 
OrderId 
{ 
get !
;! "
}# $
} 
} Ë
eD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Domain.Events\OnYarnAddDocument.cs
	namespace 	
Manufactures
 
. 
Domain 
. 
Events $
{ 
public 

class 
OnYarnAddDocument "
:# $
IManufactureEvent% 6
{ 
public 
OnYarnAddDocument  
(  !
Guid! %
id& (
)( )
{ 	
Id		 
=		 
id		 
;		 
}

 	
public 
Guid 
Id 
{ 
get 
; 
} 
} 
} þ
nD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Manufactures.Domain.Events\OnYarnNumberDocumentPlaced.cs
	namespace 	
Manufactures
 
. 
Domain 
. 
Events $
{ 
public 

class &
OnYarnNumberDocumentPlaced +
:, -
IManufactureEvent. ?
{ 
public &
OnYarnNumberDocumentPlaced )
() *
Guid* .
identity/ 7
)7 8
{ 	
Identity		 
=		 
identity		 
;		  
}

 	
public 
Guid 
Identity 
{ 
get "
;" #
}$ %
} 
} 