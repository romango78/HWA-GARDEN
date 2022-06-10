\usepackage{mathtools}

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
A' = [[A<sub>1</sub>,A<sub>2</sub>]] = $\langle f \rangle$
