# Intoduction

TBD

## Compositional Patterns for Long–running Transactions

TBD

### Sequential Transactions

Activities A<sub>1</sub>,... ,A<sub>n</sub> composing a sequential transaction are assumed 
to be executed sequentially, namely, when activity A<sub>i</sub> is committed, 
activity A<sub>i+1</sub> starts its execution. Compensation activities B<sub>1</sub>,... ,
B<sub>n</sub> are associated with each activity A<sub>i</sub>. Transactions of this kind must 
be guaranteed that either the entire sequence A<sub>1</sub>,... ,A<sub>n</sub> is executed or 
the compensated sequence A<sub>1</sub>,... ,A<sub>i</sub>,B<sub>i</sub>,... ,B<sub>1</sub> 
is executed for some i < n.

# Saga Definitions

## Create Event Saga

TBD

### Sequential Transaction definition

$A$ = [[$A'$┌►$B'$ $\cdot$ $A''$┌►$B''$ $\cdot$ A<sub>5</sub>]]<sup>_S_</sup>
= $\langle$($\emptyset$,$\emptyset$,{s$'$,s$''$,s<sub>5</sub>,s$'''$},{s$'$,s$''$,s$'''$,s<sub>5</sub>,q<sub>0</sub>,$\odot,\otimes$},q<sub>0</sub>,$\delta$),$\mu$$\rangle$</br>
_with_ $\delta$ = {(q<sub>0</sub>,$\tau$,true,$\emptyset$,s$'$),(s$'$,$\boxed{\cdot}$,s$''$),(s$'$,$\boxed{\times},\otimes$),(s$''$,$\boxed{\cdot}$,s<sub>5</sub>),(s$''$,$\boxed{\times}$,s$''''$),(s<sub>5</sub>,$\boxed{\cdot}$,$\odot$),(s<sub>5</sub>,$\boxed{\times}$,s$'''$),(s$'''$,$\boxed{\cdot}$,s$''''$)}</br>
$\mu$={($A'$,s$'$),($A''$,s$''$),(A<sub>5</sub>,s<sub>5</sub>),($B''$,s$'''$),($B'$,s$''''$)}

$A'$ = [[A<sub>1</sub> $\cdot$ A<sub>2</sub>┌►$B'$]]<sup>_S_</sup> 
= $\langle$({{an<sub>1</sub>!,cn<sub>1</sub>!},$\emptyset$,{s<sub>1</sub>,s<sub>2</sub>},{s<sub>1</sub>,s<sub>2</sub>,q<sub>0</sub>,q<sub>1</sub>,q<sub>2</sub>,$\odot,\otimes$},q<sub>0</sub>,$\delta'$},$\mu'$)$\rangle$ </br>
_with_ $\delta'$= {(q<sub>0</sub>,$\tau$,true,$\emptyset$,s<sub>1</sub>),(s<sub>1</sub>,$\boxed{\cdot}$,q<sub>1</sub>),(q<sub>1</sub>,an<sub>1</sub>!,true,$\emptyset$,$\odot$),(s<sub>1</sub>,$\boxed{\times}$,s<sub>2</sub>), (s<sub>2</sub>,$\boxed{\cdot}$,q<sub>2</sub>),(s<sub>2</sub>,$\boxed{\times}$,$\otimes$),(q<sub>2</sub>,cn<sub>1</sub>!,true,$\emptyset$,$\odot$))</br>
$\mu'$ = {(A<sub>1</sub>,s<sub>1</sub>),(A<sub>2</sub>,s<sub>2</sub>)}

$A''$ = [[A<sub>3</sub> $\cdot$ A<sub>4</sub>┌►$B''$]]<sup>_S_</sup> 
= $\langle$({{an<sub>2</sub>!,cn<sub>2</sub>!},$\emptyset$,{s<sub>3</sub>,s<sub>4</sub>},{s<sub>3</sub>,s<sub>4</sub>,q<sub>0</sub>,q<sub>3</sub>,q<sub>4</sub>,$\odot,\otimes$},q<sub>0</sub>,$\delta''$},$\mu''$)$\rangle$ </br>
_with_ $\delta''$ = {(q<sub>0</sub>,$\tau$,true,$\emptyset$,s<sub>3</sub>),(s<sub>3</sub>,$\boxed{\cdot}$,q<sub>3</sub>),(q<sub>3</sub>,an<sub>2</sub>!,true,$\emptyset$,$\odot$),(s<sub>3</sub>,$\boxed{\times}$,s<sub>4</sub>), (s<sub>4</sub>,$\boxed{\cdot}$,q<sub>4</sub>),(s<sub>4</sub>,$\boxed{\times}$,$\otimes$),(q<sub>4</sub>,cn<sub>2</sub>!,true,$\emptyset$,$\odot$))</br>
$\mu''$ = {(A<sub>3</sub>,s<sub>3</sub>),(A<sub>4</sub>,s<sub>4</sub>)}

$B''$ = [[B<sub>2</sub>]]<sup>_S_</sup><sub>_C_</sub> 
= $\langle$({{an<sub>2</sub>?,cn<sub>2</sub>?},$\emptyset$,{s<sub>6</sub>},{s<sub>6</sub>,q<sub>0</sub>,$\odot$},q<sub>0</sub>,$\delta'''$},$\mu'''$)$\rangle$ </br>
_with_ $\delta'''$ = {(q<sub>0</sub>,an<sub>2</sub>?,true,$\emptyset$,$\odot$),(q<sub>0</sub>,cn<sub>2</sub>?,true,$\emptyset$,s<sub>6</sub>),(s<sub>6</sub>,$\boxed{\cdot}$,$\odot$)}</br>
$\mu'''$ = {(B<sub>2</sub>,s<sub>6</sub>)}

$B'$ = [[B<sub>1</sub>]]<sup>_S_</sup><sub>_C_</sub> 
= $\langle$({{an<sub>1</sub>?,cn<sub>1</sub>?},$\emptyset$,{s<sub>7</sub>},{s<sub>7</sub>,q<sub>0</sub>,$\odot$},q<sub>0</sub>,$\delta''''$},$\mu''''$)$\rangle$ </br>
_with_ $\delta''''$ = {(q<sub>0</sub>,an<sub>1</sub>?,true,$\emptyset$,$\odot$),(q<sub>0</sub>,cn<sub>1</sub>?,true,$\emptyset$,s<sub>7</sub>),(s<sub>7</sub>,$\boxed{\cdot}$,$\odot$)}</br>
$\mu''''$ = {(B<sub>1</sub>,s<sub>7</sub>)}

_States:_</br>
q<sub>0</sub> ::= _Initial_</br>
q<sub>1</sub> ::= _Calendar exists and data was received, no compensation action is needed_</br>
q<sub>2</sub> ::= _Calendar doesn't exist and new record was created, the compensation action is needed_</br>
q<sub>3</sub> ::= _Event Group exists and data was received, no compensation action is needed_</br>
q<sub>4</sub> ::= _Event Group doesn't exist and new record was created, the compensation action is needed_</br>
s<sub>1</sub> ::= _Tryint to get exist Calendar by (Name, Year)_</br>
s<sub>2</sub> ::= _Trying to add a new Calendar record_</br>
s<sub>3</sub> ::= _Tryint to get exist Event Group by (Name)_</br>
s<sub>4</sub> ::= _Trying to add a new Event Group record_</br>
s<sub>5</sub> ::= _Trying to add a new Event record_</br>
s<sub>6</sub> ::= _Trying to remove added Event Group record (compensation action for A<sub>4</sub>)_</br>
s<sub>7</sub> ::= _Trying to remove added Calendar record (compensation action for A<sub>2</sub>)_</br>

![Fig. 1. Create Event STD Pattern](Create Event LRT.drawio.svg)