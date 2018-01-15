# BT commands # {#bt_commands}

N.B. Behaviour Trees in Fungus should be considered experimental and beta. Feed back is welcome, desired and highly encouraged. As such, there may be breaking changes in future releases.

[TOC]
# Fail # {#Fail}
Sets this block's execution state / status to failed and stops the block

Defined in Fungus.FailBlock
# ForceResult # {#ForceResult}
Returns the specified result regardless of the target block's status.

Defined in Fungus.ForceResult

Property | Type | Description
 --- | --- | ---
Is Forcing Success | System.Boolean | Force result to success, if false forces failure.
Block | Fungus.Block | Block to call.

# Invert # {#Invert}
Inverts the success or failure of the called block

Defined in Fungus.InvertBT

Property | Type | Description
 --- | --- | ---
Block | Fungus.Block | Block to call.

# Parallel # {#Parallel}
Execute blocks until one succeeds, return fail if all fail

Defined in Fungus.Parallel

Property | Type | Description
 --- | --- | ---
Target Blocks | System.Collections.Generic.List`1[Fungus.Block] | Blocks to executing, order is important.
Shuffle Commands | Fungus.BooleanData | Shuffle will reorder the sub blocks on every execution

# Repeat # {#Repeat}
Repeats the sub block until a certain condition is met.

Defined in Fungus.RepeatBT

Property | Type | Description
 --- | --- | ---
Repeat Until | Fungus.BehaviourState | Continue to call the target block until it returns this state.
Time Between Repeats | System.Single | Seconds to wait before next call, after a return does not meet termination state.
Block | Fungus.Block | Block to call.

# Selector # {#Selector}
Execute blocks until one succeeds, return fail if all fail

Defined in Fungus.Selector

Property | Type | Description
 --- | --- | ---
Target Blocks | System.Collections.Generic.List`1[Fungus.Block] | Blocks to executing, order is important.
Shuffle Commands | Fungus.BooleanData | Shuffle will reorder the sub blocks on every execution

# Sequence # {#Sequence}
Execute blocks until one fails, return success if all are successful

Defined in Fungus.Sequence

Property | Type | Description
 --- | --- | ---
Target Blocks | System.Collections.Generic.List`1[Fungus.Block] | Blocks to executing, order is important.
Shuffle Commands | Fungus.BooleanData | Shuffle will reorder the sub blocks on every execution

