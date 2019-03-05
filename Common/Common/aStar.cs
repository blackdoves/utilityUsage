using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    class Point
    {
        //structure

        public int y;
        public int x;
        public int G;
        public int H;

        public Point(int x0, int y0, int G0, int H0, Point F)
        {
            x = x0;
            y = y0;
            G = G0;
            H = H0;
            father = F;
        }
        public Point father;

    }
    class aStar
    {
        //open close list
        List<Point> open_list = new List<Point>();
        List<Point> close_list = new List<Point>();
        


        //find min nod from open list


        //whether a obstacle 


        //open list is contain point 

        //get point from close list


        //grt point from open list

        //count G

        //count H

        //check nearby point 

        //find way 

        //save way

        //print map 

    }
}
