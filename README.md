# YellowstoneDrones
Control Forest Fires

The Yellowstone’s park control forest fires team is setting up a group of drones for overflying 
the park area performing heat detection  

For controlling the movement area, the number of drones and its movements the engineering team 
has designed the following system.

    1. We send all the instructions at once.
    2. First line defines the flying area, specified by a rectangle representing the drones movement area.
    3. Next line defines the dron start position and its direction "N", "E", "S" o "W". 
       For instance “0 0 N” will set the initial dron position to the left bottom of the rectangle area 
       oriented to the North.
    4. Next line defines the dron movement action. The movements are codified by letters. “L” and “R” for 
       turning left or right the drone 90º. “M” for moving forward one position the dron, for instance 
       if the drone is moving towards north it will go from (x,y) to (x,y+1).
    5. We can go on adding as many as pair instructions following the same specification explained in 
       the points 3 and 4.
    6. Finally, after doing all the movements every drone will output its current position and direction 
        in the same format as the start position was provided. 

Sample:

Input:
5 5
3 3 E
L
3 3 E
MMRMMRMRRM
1 2 N
LM LM LM LM ML ML ML ML MM

Output:
3 3 N
5 1 E
1 4 N

Important Note:
Apart from the correct solution we are also looking for clean, tested and documented code. Would be perfect 
if you provide an easy to follow readme file with the execution and testing instructions.

