A*自动寻路算法
=======================================================
- 下面给出几个概念:
  
  - `搜索区域(The Search Area)`：图中的搜索区域被划分为了简单的二维数组,数组每个元素对应一个小方格,当然我们也可以将区域等分成是五角星,矩形等.通常将一个单位的中心点称之为`搜索区域节点(Node)`.
  
  - `开放列表(Open List)`：我们将路径规划过程中待检测的节点存放于`Open List`中,而已检测过的格子则存放于`Close List`中.
  
  - `父节点(parent)`：在路径规划中用于回溯的节点，开发时可考虑为双向链表结构中的父结点指针.
  
  - `路径排序(Path Sorting)`：具体往哪个节点移动由以下公式确定：`F(n) = G + H`. G代表的是从初始位置A沿着已生成的路径到指定待检测格子的移动开销,H指定待测格子到目标节点B的估计移动开销.
  
  - `启发函数(Heuristics Function)`：H为启发函数,也被认为是一种试探,由于在找到唯一路径前,我们不确定在前面会出现什么障碍物,因此用了一种计算H的算法,具体根据实际场景决定.在我们简化的模型中,H采用的是传统的`曼哈顿距离(Manhattan Distance)`,也就是横纵向走的距离之和.

  ![Image text](https://github.com/Perry961002/Dynamic_Demonstration_Of_A-Start_Algorithm/blob/master/AStart.png?raw=true)
  
- 比较官方的伪算法描述如下：

 function A*(start, goal)
 
    // The set of nodes already evaluated.
    closedSet := {}
    // The set of currently discovered nodes still to be evaluated. Initially, only the start node is known.
    openSet := {start}
    // For each node, which node it can most efficiently be reached from.
    // If a node can be reached from many nodes, cameFrom will eventually contain the most efficient previous step.
    cameFrom := the empty map
    // For each node, the cost of getting from the start node to that node.
    gScore := map with default value of Infinity
    // The cost of going from start to start is zero.
    gScore[start] := 0 
    // For each node, the total cost of getting from the start node to the goal
    // by passing by that node. That value is partly known, partly heuristic.
    fScore := map with default value of Infinity
    // For the first node, that value is completely heuristic.
    fScore[start] := heuristic_cost_estimate(start, goal)
    while openSet is not empty
        current := the node in openSet having the lowest fScore[] value
        if current = goal
            return reconstruct_path(cameFrom, current)
        openSet.Remove(current)
        closedSet.Add(current)
        for each neighbor of current
            if neighbor in closedSet
                continue        // Ignore the neighbor which is already evaluated.
            // The distance from start to a neighbor
            tentative_gScore := gScore[current] + dist_between(current, neighbor)
            if neighbor not in openSet    // Discover a new node
                openSet.Add(neighbor)
            else if tentative_gScore >= gScore[neighbor]
                continue        // This is not a better path.

            // This path is the best until now. Record it!
            cameFrom[neighbor] := current
            gScore[neighbor] := tentative_gScore
            fScore[neighbor] := gScore[neighbor] + heuristic_cost_estimate(neighbor, goal)
    return failure
    
function reconstruct_path(cameFrom, current)

    total_path := [current]
    while current in cameFrom.Keys:
        current := cameFrom[current]
        total_path.append(current)
    return total_path
