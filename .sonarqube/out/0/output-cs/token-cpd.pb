÷
[D:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Infrastructure.Events\EntityEvents.cs
	namespace 	
Infrastructure
 
. 
Domain 
.  
Events  &
{ 
public 

class 
OnEntityCreated  
<  !
T! "
>" #
:$ %
IDomainEvent& 2
{ 
public 
OnEntityCreated 
( 
T  
entity! '
)' (
{ 	
Entity 
= 
entity 
; 
} 	
public

 
T

 
Entity

 
{

 
get

 
;

 
}

  
} 
public 

class 
OnEntityUpdated  
<  !
T! "
>" #
:$ %
IDomainEvent& 2
{ 
public 
OnEntityUpdated 
( 
T  
entity! '
)' (
{ 	
Entity 
= 
entity 
; 
} 	
public 
T 
Entity 
{ 
get 
; 
}  
} 
public 

class 
OnEntityDeleted  
<  !
T! "
>" #
:$ %
IDomainEvent& 2
{ 
public 
OnEntityDeleted 
( 
T  
entity! '
)' (
{ 	
Entity 
= 
entity 
; 
} 	
public 
T 
Entity 
{ 
get 
; 
}  
} 
}   û
[D:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Infrastructure.Events\IDomainEvent.cs
	namespace 	
Infrastructure
 
. 
Domain 
.  
Events  &
{ 
public 

	interface 
IDomainEvent !
:" #
INotification$ 1
{ 
} 
} Ä
bD:\DanLirisDevelopment\id.co.danliris-weaving-web\src\Infrastructure.Events\IDomainEventHandler.cs
	namespace 	
Infrastructure
 
. 
Domain 
.  
Events  &
{ 
public 

	interface 
IDomainEventHandler (
<( )
TEvent) /
>/ 0
:1 2 
INotificationHandler3 G
<G H
TEventH N
>N O
whereP U
TEventV \
:] ^
IDomainEvent_ k
{ 
} 
} 