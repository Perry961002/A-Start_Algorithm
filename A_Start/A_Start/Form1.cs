using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace A_Start
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        #region 数据集合
        Thread shThread = null;
        //障碍物集
        List<Grid> stoneGridList = new List<Grid>();
        //Open格子集合
        List<Grid> openGridList = new List<Grid>();
        //Closed格子集合
        List<Grid> closedGridList = new List<Grid>();
        //起始点
        Grid startGrid = null;
        //终点
        Grid endGrid = null;
        //上下左右格子
        Grid up_grid, down_grid, left_grid, right_grid;
        //标志是不是添加障碍物
        bool isAddStone = false;
        #endregion
        //画网格
        public void DrawMap()
        {
            Point pt1;
            Point pt2;
            Pen pen = new Pen(Color.Black);
            for (int i = 0; i < 20; i++)
            {
                pt1 = new Point(0, i * 20);
                pt2 = new Point(400, i * 20);
                this.PanelMap.CreateGraphics().DrawLine(pen, pt1,pt2);
            }
            for (int i = 0; i < 20; i++)
            {
                pt1 = new Point(i * 20, 0);
                pt2 = new Point(i * 20, 400);
                this.PanelMap.CreateGraphics().DrawLine(pen, pt1, pt2);
            }
        }

        //画障碍物
        private void DrawStone(Point p)
        {
            Grid stone = new Grid();
            stone.Location = p;
            stone.ColourGrid = Color.Black;//障碍物颜色为黑
            stone.State = Grid.GridState.stone;
            this.PanelMap.CreateGraphics().FillRectangle(new SolidBrush(stone.ColourGrid), stone.Location.X + 1, stone.Location.Y + 1, 19, 19);
            this.stoneGridList.Add(stone);
        }

        //产生随机障碍物
        private void CreateStone()
        {
            Random ra = new Random();
            for(int i=0;i<20;i++)
            {
                //产生障碍物的坐标
                int x = ra.Next(20);
                int y = ra.Next(20);
                Point p = new Point();
                p.X = x * 20;
                p.Y = y * 20;
                //障碍物集合有东西
                if (stoneGridList.Count != 0)
                {
                    foreach(Grid grid in stoneGridList)
                    {
                        //不能产生已经出现过的障碍物
                        if (p == grid.Location)
                            continue;
                    }
                    //位置不同就画
                    this.DrawStone(p);
                    continue;
                }
                this.DrawStone(p);
            }
        }

        //地图的描绘事件
        private void PanelMap_Paint(object sender, PaintEventArgs e)
        {
            this.DrawMap();
            this.CreateStone();
        }

        //鼠标在网格的点击事件
        private void PanelMap_MouseDown(object sender, MouseEventArgs e)
        {
            int row, clo;
            //记录按下点的坐标
            clo = e.X / 20;
            row = e.Y / 20;
            Point p = new Point(clo * 20, row * 20);
            Grid grid = new Grid();
            grid.Location = p;
            //是不是添加障碍物
            if(isAddStone)
            {
                //添加的不能是添加过的地方
                foreach(Grid g in stoneGridList)
                {
                    if (grid.HasGrid(g, p))
                        return;
                }
                this.DrawStone(p);
                return;
            }
            //画起始点和终点
            //先检查这个点是不是障碍物
            foreach(Grid g in stoneGridList)
            {
                if(grid.HasGrid(g, p))
                {
                    MessageBox.Show("请另选方格！");
                    return;
                }
            }
            //先画起点
            if(startGrid == null)
            {
                grid.ColourGrid = Color.Blue;//起点为蓝色
                grid.State = Grid.GridState.start;
                startGrid = grid;
                this.PanelMap.CreateGraphics().FillRectangle(new SolidBrush(grid.ColourGrid), grid.Location.X + 1, grid.Location.Y + 1, 19, 19);
                //障碍点按钮不可用
                this.AddStones.Enabled = false;
                return;
            }
            //再画终点
            if(endGrid == null)
            {
                grid.ColourGrid = Color.Red;//终点为红色
                grid.State = Grid.GridState.end;
                endGrid = grid;
                this.PanelMap.CreateGraphics().FillRectangle(new SolidBrush(grid.ColourGrid), grid.Location.X + 1, grid.Location.Y + 1, 19, 19);
            }
        }

        //手动添加障碍
        private void AddStones_Click(object sender, EventArgs e)
        {
            if (this.AddStones.Text == "手动添加障碍物")
            {
                this.AddStones.Text = "取消添加障碍物";
                this.AddStones.ForeColor = Color.Red;
                isAddStone = true;//设置标志
            }
            else
            {
                isAddStone = false;
                this.AddStones.Text = "手动添加障碍物";
                this.AddStones.ForeColor = Color.Black;
            }
        }

        //开始演示算法
        private void Action_Click(object sender, EventArgs e)
        {
            if(startGrid == null)
            {
                MessageBox.Show("缺少起点！");
                return;
            }
            if(endGrid == null)
            {
                MessageBox.Show("缺少终点！");
                return;
            }
            this.Reset.Enabled = false;
            this.Action.Enabled = false;
            //获取起始点的估计代价
            startGrid.EvaluatePrice = this.GetEvaluatePrice(startGrid);
            //把起始点放入Closed集合中
            this.closedGridList.Add(startGrid);
            //求解进程开始
            shThread = new Thread(new ThreadStart(this.BeginSearch));
            shThread.Start();
            Console.WriteLine(closedGridList.Count);
        }
        #region A*算法的函数
        //估算代价
        public int GetEvaluatePrice(Grid grid)
        {
            //采用哈密顿距离
            int x = Math.Abs(grid.Location.X - endGrid.Location.X) / 20;
            int y = Math.Abs(grid.Location.Y - endGrid.Location.Y) / 20;
            //距离之和
            int price = x + y;
            return price;
        }

        //判断格子在不在Closed表中,在就返回true
        public bool isInClosed(Grid grid)
        {
            foreach(Grid g in closedGridList)
            {
                //找到同样的点了
                if (grid.isSameGrid(grid, g))
                    return true;
            }
            return false;
        }

        //判断格子在不在Open表中,在返回true
        public bool isInOpen(Grid grid)
        {
            foreach (Grid g in closedGridList)
            {
                //找到同样的点了
                if (grid.isSameGrid(grid, g))
                    return true;
            }
            return false;
        }

        //判断是否出界,是否是障碍,不是的返回true
        public bool isStoneOrOut(Grid grid)
        {
            if(grid.Location.X >=0 && grid.Location.X < this.PanelMap.Width && grid.Location.Y >= 0 && grid.Location.Y < this.PanelMap.Height)
            {
                foreach(Grid g in stoneGridList)
                {
                    if (grid.isSameGrid(grid, g))
                        return false;
                }
                return true;
            }
            return false;
        }

        //寻找估计代价最小的格子
        public void SearchBestGrid()
        {
            //初始化4个方向
            up_grid = new Grid(); down_grid = new Grid();
            left_grid = new Grid(); right_grid = new Grid();
            //从Closed表的末尾节点构造4个方向
            up_grid.Location = new Point(closedGridList[closedGridList.Count - 1].Location.X, closedGridList[closedGridList.Count - 1].Location.Y - 20);
            down_grid.Location = new Point(closedGridList[closedGridList.Count - 1].Location.X, closedGridList[closedGridList.Count - 1].Location.Y + 20);
            left_grid.Location = new Point(closedGridList[closedGridList.Count - 1].Location.X - 20, closedGridList[closedGridList.Count - 1].Location.Y);
            right_grid.Location = new Point(closedGridList[closedGridList.Count - 1].Location.X + 20, closedGridList[closedGridList.Count - 1].Location.Y);
            //判断4个方格是否在边界或者是障碍物,不是的话加入Open表中
            if(this.isStoneOrOut(up_grid) && !isInClosed(up_grid) && !isInOpen(up_grid))
            {
                //更新父亲节点
                up_grid.ParentGrid = closedGridList[closedGridList.Count - 1];
                //更新实际代价
                up_grid.RealPrice = up_grid.ParentGrid.RealPrice + 1;
                //更新估计代价
                up_grid.EvaluatePrice = this.GetEvaluatePrice(up_grid);
                //更新最终代价
                up_grid.FinalPrice = up_grid.EvaluatePrice + up_grid.RealPrice;
                //加入Open表
                this.openGridList.Add(up_grid);
                this.PanelMap.CreateGraphics().DrawString("开", new Font("黑体", 10), new SolidBrush(Color.Red), up_grid.Location);
                Thread.Sleep(100);//演示显示
            }
            if (this.isStoneOrOut(down_grid) && !isInClosed(down_grid) && !isInOpen(down_grid))
            {
                //更新父亲节点
                down_grid.ParentGrid = closedGridList[closedGridList.Count - 1];
                //更新实际代价
                down_grid.RealPrice = down_grid.ParentGrid.RealPrice + 1;
                //更新估计代价
                down_grid.EvaluatePrice = this.GetEvaluatePrice(down_grid);
                //更新最终代价
                down_grid.FinalPrice = down_grid.EvaluatePrice + down_grid.RealPrice;
                //加入Open表
                this.openGridList.Add(down_grid);
                this.PanelMap.CreateGraphics().DrawString("开", new Font("黑体", 10), new SolidBrush(Color.Red), down_grid.Location);
                Thread.Sleep(100);//演示显示
            }
            if (this.isStoneOrOut(left_grid) && !isInClosed(left_grid) && !isInOpen(left_grid))
            {
                //更新父亲节点
                left_grid.ParentGrid = closedGridList[closedGridList.Count - 1];
                //更新实际代价
                left_grid.RealPrice = left_grid.ParentGrid.RealPrice + 1;
                //更新估计代价
                left_grid.EvaluatePrice = this.GetEvaluatePrice(left_grid);
                //更新最终代价
                left_grid.FinalPrice = left_grid.EvaluatePrice + left_grid.RealPrice;
                //加入Open表
                this.openGridList.Add(left_grid);
                this.PanelMap.CreateGraphics().DrawString("开", new Font("黑体", 10), new SolidBrush(Color.Red), left_grid.Location);
                Thread.Sleep(100);//演示显示
            }
            if (this.isStoneOrOut(right_grid) && !isInClosed(right_grid) && !isInOpen(right_grid))
            {
                //更新父亲节点
                right_grid.ParentGrid = closedGridList[closedGridList.Count - 1];
                //更新实际代价
                right_grid.RealPrice = right_grid.ParentGrid.RealPrice + 1;
                //更新估计代价
                right_grid.EvaluatePrice = this.GetEvaluatePrice(right_grid);
                //更新最终代价
                right_grid.FinalPrice = right_grid.EvaluatePrice + right_grid.RealPrice;
                //加入Open表
                this.openGridList.Add(right_grid);
                this.PanelMap.CreateGraphics().DrawString("开", new Font("宋体", 10), new SolidBrush(Color.Red), right_grid.Location);
                Thread.Sleep(100);//演示显示
            }
            List<Grid> minGrids = new List<Grid>();
            //记录最小代价
            List<Grid> minestGrids = new List<Grid>();
            //最小的最终代价和估计代价
            int minF = 1000, minE = 1000;
            //在Open集合中找到最小的F(n)值
            foreach(Grid g in openGridList)
            {
                if (g.FinalPrice <= minF)
                    minF = g.FinalPrice;
            }
            //找到F(n)最小的格子
            foreach(Grid g in openGridList)
            {
                if (g.FinalPrice == minF)
                    minGrids.Add(g);
            }
            //在F(n)值最小的格子集合中找出最小的G(n)值
            foreach(Grid g in minGrids)
            {
                if (g.EvaluatePrice <= minE)
                    minE = g.EvaluatePrice;
            }
            //在F(n)值最小的格子集合中找出G(n)值最小的格子
            foreach(Grid g in minGrids)
            {
                if(g.EvaluatePrice == minE)
                {
                    minestGrids.Add(g);
                }
            }
            Random rd = new Random();
            ////在F(n)值和G(n)的格子集合中随机选取一个格子
            if(minestGrids.Count == 0)
            {
                MessageBox.Show("哈哈哈，没找到");
                return;
            }
            //选一个格子加入Closed集合
            int j = rd.Next(minestGrids.Count);
            minestGrids[j].ColourGrid = Color.Purple;
            this.closedGridList.Add(minestGrids[j]);
            this.PanelMap.CreateGraphics().FillRectangle(new SolidBrush(Color.WhiteSmoke), this.closedGridList[closedGridList.Count - 1].Location.X + 1, this.closedGridList[closedGridList.Count - 1].Location.Y + 1, 19, 19);
            this.PanelMap.CreateGraphics().DrawString("关" , new Font("宋体", 10), new SolidBrush(Color.Red), this.closedGridList[closedGridList.Count - 1].Location);
            Thread.Sleep(150);
            this.PanelMap.CreateGraphics().FillRectangle(new SolidBrush(Color.Purple), this.closedGridList[closedGridList.Count - 1].Location.X + 1, this.closedGridList[closedGridList.Count - 1].Location.Y + 1, 19, 19);
            //从open集合中移除刚刚添加到closed集合中的格子
            foreach(Grid g in openGridList)
            {
                if(g.isSameGrid(g, minestGrids[j]))
                {
                    openGridList.Remove(g);
                    break;
                }
            }
        }

        //开始搜索
        public void BeginSearch()
        {
            //不停搜索直到到达目的地
            while(true)
            {
                this.SearchBestGrid();
                Grid closedGrid = this.closedGridList[closedGridList.Count - 1];
                Grid _upGrid = new Grid(); Grid _downGrid = new Grid();
                Grid _leftGrid = new Grid(); Grid _rightGrid = new Grid();
                _upGrid.Location = new Point(closedGrid.Location.X, closedGrid.Location.Y + 20);
                _downGrid.Location = new Point(closedGrid.Location.X, closedGrid.Location.Y - 20);
                _leftGrid.Location = new Point(closedGrid.Location.X - 20, closedGrid.Location.Y);
                _rightGrid.Location = new Point(closedGrid.Location.X + 20, closedGrid.Location.Y + 20);
                //已经到达终点
                if (_upGrid.isSameGrid(_upGrid, endGrid) || _downGrid.isSameGrid(_downGrid, endGrid) || _leftGrid.isSameGrid(_leftGrid, endGrid) || _rightGrid.isSameGrid(_rightGrid, endGrid))
                    break;
            }
            //最优路径
            List<Grid> bestRoad = new List<Grid>();
            Grid lastClosedGrid = new Grid();
            int minE = 1000;
            foreach(Grid g in this.closedGridList)
            {
                if (g.EvaluatePrice < minE)
                    minE = g.EvaluatePrice;
            }
            foreach (Grid g in this.closedGridList)
            {
                if (g.EvaluatePrice == minE)
                {
                    lastClosedGrid = g;
                    break;
                }
             }
            //逆向把最优路径装入List
            while(true)
            {
                if(lastClosedGrid.isSameGrid(lastClosedGrid.ParentGrid, startGrid))
                {
                    bestRoad.Add(lastClosedGrid);
                    break;
                }
                else
                {
                    bestRoad.Add(lastClosedGrid);
                    lastClosedGrid = lastClosedGrid.ParentGrid;
                }
            }
            foreach(Grid g in bestRoad)
            {
                this.PanelMap.CreateGraphics().FillRectangle(new SolidBrush(Color.Green), g.Location.X + 1, g.Location.Y + 1, 19, 19);
                Thread.Sleep(100);
            }
            //恢复重置按钮
            this.Reset.Enabled = true;
        }
        #endregion

        //重置按钮
        private void Reset_Click(object sender, EventArgs e)
        {
            //各种重置颜色和清空
            if(startGrid != null)
            {
                this.PanelMap.CreateGraphics().FillRectangle(new SolidBrush(Color.WhiteSmoke), startGrid.Location.X+1, startGrid.Location.Y+1, 19, 19);
                startGrid = null;
            }
            if (endGrid != null)
            {
                this.PanelMap.CreateGraphics().FillRectangle(new SolidBrush(Color.WhiteSmoke), endGrid.Location.X + 1, endGrid.Location.Y + 1, 19, 19);
                endGrid = null;
            }
            foreach (Grid g in stoneGridList)
                this.PanelMap.CreateGraphics().FillRectangle(new SolidBrush(Color.WhiteSmoke), g.Location.X + 1, g.Location.Y + 1, 19, 19);
            stoneGridList.Clear();
            foreach (Grid g in closedGridList)
                this.PanelMap.CreateGraphics().FillRectangle(new SolidBrush(Color.WhiteSmoke), g.Location.X + 1, g.Location.Y + 1, 19, 19);
            foreach (Grid g in openGridList)
                this.PanelMap.CreateGraphics().FillRectangle(new SolidBrush(Color.WhiteSmoke), g.Location.X + 1, g.Location.Y + 1, 19, 19);
            closedGridList.Clear(); openGridList.Clear();
            this.AddStones.Enabled = true;
            this.Action.Enabled = true;
            this.CreateStone();
        }

        //关闭窗体
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (shThread != null)
                shThread.Abort();
        }

    }
}
