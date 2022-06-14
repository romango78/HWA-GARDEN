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

A = </br>
A' = [[A<sub>1</sub> $\cdot$ A<sub>2</sub>┌►B<sub>1</sub>]]<sup>_S_</sup> 
= $\langle$(
{an<sub>1</sub>!,cn<sub>1</sub>!},
$\emptyset$,
{s<sub>1</sub>,s<sub>2</sub>},
{s<sub>1</sub>,s<sub>2</sub>,q<sub>0</sub>,q<sub>1</sub>,q<sub>2</sub>,$\odot,\otimes$},
q<sub>0</sub>,
$\delta$'},$\mu$')$\rangle$ </br>
_with_ $\delta$'= {(q<sub>0</sub>,$\tau$,true,$\emptyset$,s<sub>1</sub>),(s<sub>1</sub>,$\boxed{\cdot}$,q<sub>1</sub>),(q<sub>1</sub>,an<sub>1</sub>!,true,$\emptyset$,$\odot$),(s<sub>1</sub>,$\boxed{\times}$,s<sub>2</sub>), (s<sub>2</sub>,$\boxed{\cdot}$,q<sub>2</sub>),(s<sub>2</sub>,$\boxed{\times}$,$\otimes$),(q<sub>2</sub>,cn<sub>1</sub>!,true,$\emptyset$,$\odot$))</br>
$\mu$'={(A<sub>1</sub>,s<sub>1</sub>),(A<sub>2</sub>,s<sub>2</sub>)}

_States:_</br>
q<sub>0</sub> ::= _Initial_</br>
q<sub>1</sub> ::= _Calendar exists and data was received, no compensation action is needed_</br>
q<sub>2</sub> ::= _Calendar doesn't exist and new record was created, the compensation action is needed_</br>
s<sub>1</sub> ::= _Tryint to get exist calendar by (Name, Year)_</br>
s<sub>2</sub> ::= _Trying to add a new record for Calendar_</br>
