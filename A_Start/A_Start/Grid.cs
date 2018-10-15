using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace A_Start
{
    public class Grid
    {
        //位置
        private Point location;
        //编号
        private int id;
        //实际代价
        private int realPrice;
        //估计代价
        private int evaluatePrice;
        //最终代价
        private int finalPrice;
        //父亲格子
        private Grid parentGrid;
        //上下左右格子
        private Grid upGrid, downGrid, leftGrid, rightGrid;
        //格子颜色
        private Color GridColor;
        //格子状态
        private GridState gridState;
        public enum GridState
        {
            start, end, stone, road
        }
        
        #region Get和Set方法
        public Point Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
            }
        }

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public int RealPrice
        {
            get {return realPrice;}
            set {realPrice = value;}
        }

        public int EvaluatePrice
        {
            get {return evaluatePrice;}
            set {evaluatePrice = value;}
        }
        
        public int FinalPrice
        {
            get {return finalPrice;}
            set {finalPrice = value;}
        }

        public Grid ParentGrid
        {
            get {return parentGrid;}
            set {parentGrid = value;}
        }
        public Grid UpGrid
        {
            get {return upGrid;}
            set {upGrid = value;}
        }
        public Grid DownGrid
        {
            get {return downGrid;}
            set {downGrid = value;}
        }
        public Grid LeftGrid
        {
            get {return leftGrid;}
            set {leftGrid = value;}
        }
        public Grid RightGrid
        {
            get {return rightGrid;}
            set {rightGrid = value;}
        }

        public Color ColourGrid
        {
            get {return GridColor;}
            set {GridColor = value;}
        }

        public GridState State
        {
            get {return gridState;}
            set {gridState = value;}
        }
        #endregion

        //判断是否有格子,是就返回true
        public bool HasGrid(Grid grid, Point p)
        {
            //格子的位置和p的坐标相等
            if(grid.location == p)
                return true;
            else
                return false;
        }

        //判断两个格子是不是同一个,是就返回true
        public bool isSameGrid(Grid g1, Grid g2)
        {
            if(g1.location == g2.location)
                return true;
            else
                return false;
        }

    }
}
