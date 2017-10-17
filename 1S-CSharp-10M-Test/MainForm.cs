using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1S_CSharp_10M_Test
{
    public partial class MainForm : Form
    {
        private System.Collections.Generic.List<System.Collections.Generic.List<bool>> listElevatorStatus;
        public MainForm()
        {
            InitializeComponent();

            this.initListElevator();

            this.updateElevators();
        }

        private void initListElevator()
        {
            listElevatorStatus = new System.Collections.Generic.List<System.Collections.Generic.List<bool>>();
            for (int e = 0; e < 3; e++)
            {
                System.Collections.Generic.List<bool> floorsStatus = new System.Collections.Generic.List<bool>();
                floorsStatus.Add(true);
                for (int f = 1; f < 10; f++)
                {
                    floorsStatus.Add(false);
                }
                listElevatorStatus.Add(floorsStatus);
            }
        }

        private void log(string msg)
        {
            logTextArea.Text = logTextArea.Text.Insert(0, msg + System.Environment.NewLine);
        }
        private void updateElevators()
        {
            for (int e = 0; e < 3; e++)
            {
                for (int f = 9; f >= 0; f--)
                {
                    if (listElevatorStatus[e][f])
                    {
                        listElevator.Items[9 - f].SubItems[e + 1].Text = "O";
                        System.Threading.Thread.Sleep(500);
                    }
                    else
                    {
                        listElevator.Items[9 - f].SubItems[e + 1].Text = "";
                    }
                }
            }
        }

        private int getElevatorFloor(int e)
        {
            for(int f = 0; f < 10; f++)
            {
                if (listElevatorStatus[e][f])
                {
                    return f;
                }
            }

            return -1;
        }

        private void setElevatorFloor(int e, int f)
        {
            listElevatorStatus[e][this.getElevatorFloor(e)] = false;
            listElevatorStatus[e][f] = true;

            switch(e)
            {
                case 0:
                    {
                        e1floorLabel.Text = (f + 1).ToString() + "F";
                    }
                    break;
                case 1:
                    {
                        e2floorLabel.Text = (f + 1).ToString() + "F";
                    }
                    break;
                case 2:
                    {
                        e3floorLabel.Text = (f + 1).ToString() + "F";
                    }
                    break;
                default:
                    break;
            }

            this.log("Elevator <" + (e+1).ToString() + "> went to <" + (f + 1) + "F>");
        }

        private void moveElevatorToFloor(int e, int toF)
        {
            if ((toF < 0) || (toF >= 10))
            {
                this.log("Value of 'TO' must be > 0 and < 10");
                return;
            }

            switch (e)
            {
                case 0:
                    {
                        e1doorLabel.Text = "CLOSE";
                    }
                    break;
                case 1:
                    {
                        e2doorLabel.Text = "CLOSE";
                    }
                    break;
                case 2:
                    {
                        e3doorLabel.Text = "CLOSE";
                    }
                    break;
                default:
                    break;
            }

            this.log("Elevator <" + (e + 1).ToString() + "> 's door is now CLOSE");

            int nowF = this.getElevatorFloor(e);

            if (nowF > toF)
            {
                for (int f = nowF - 1; f >= toF; f--)
                {
                    this.setElevatorFloor(e, f);
                    updateElevators();

                }
            }
            else if (nowF < toF)
            {
                for (int f = nowF + 1; f <= toF; f++)
                {
                    this.setElevatorFloor(e, f);
                    updateElevators();
                }
            }
            else
            {
                this.setElevatorFloor(e, toF);
            }

            switch (e)
            {
                case 0:
                    {
                        e1doorLabel.Text = "OPEN";
                    }
                    break;
                case 1:
                    {
                        e2doorLabel.Text = "OPEN";
                    }
                    break;
                case 2:
                    {
                        e3doorLabel.Text = "OPEN";
                    }
                    break;
                default:
                    break;
            }

            this.log("Elevator <" + (e + 1).ToString() + "> 's door is now OPEN");
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void e1upBtn_Click(object sender, EventArgs e)
        {
            int toF = (int)e1toNumBox.Value;

            moveElevatorToFloor(0, toF - 1);
            updateElevators();
        }

        private void e1downBtn_Click(object sender, EventArgs e)
        {
            this.e1upBtn_Click(sender, e);
        }

        private void e2upBtn_Click(object sender, EventArgs e)
        {
            int toF = (int)e2toNumBox.Value;

            moveElevatorToFloor(1, toF - 1);

            updateElevators();
        }

        private void e2downBtn_Click(object sender, EventArgs e)
        {

            e2upBtn_Click(sender, e);
        }

        private void e3upBtn_Click(object sender, EventArgs e)
        {
            int toF = (int)e3toNumBox.Value;

            moveElevatorToFloor(2, toF - 1);

            updateElevators();
        }

        private void e3downBtn_Click(object sender, EventArgs e)
        {
            e3upBtn_Click(sender, e);
        }

        private int getDistance(int from, int to)
        {
            int dist = from - to;

            if (dist < 0)
            {
                dist = ~dist + 1;
            }

            return dist;
        }

        private void callElevatorBtn_Click(object sender, EventArgs ev)
        {
            this.log("Caculatting elevator");

            int goodElevator = 0;

            int curfloor = (int)currentFloor.Value - 1;

            int dist_min = this.getDistance(curfloor, this.getElevatorFloor(goodElevator));

            for (int e = 0; e < 3; e++)
            {
                int dist = this.getDistance(curfloor, this.getElevatorFloor(e));
                this.log("Distance of Elevator <" + (e + 1) + "> is " + dist);

                if (dist < dist_min)
                {
                    dist_min = dist;
                    goodElevator = e;
                }
            }

            this.log("Choosen elevator <" + (goodElevator + 1) + ">");

            this.moveElevatorToFloor(goodElevator, curfloor);

            this.updateElevators();
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            this.log("Start MOVE elevators");
            e1upBtn_Click(null, null);
            e2upBtn_Click(null, null);
            e3upBtn_Click(null, null);
            this.log("Completed MOVE elevators");
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            this.log("STOP all Elevator");
            e1moveLabel.Text = "STOP";
            e2moveLabel.Text = "STOP";
            e3moveLabel.Text = "STOP";

            this.updateElevators();
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            e1moveLabel.Text = "DOWN";
            e1toNumBox.Value = 0;
            e1doorLabel.Text = "CLOSE";
            this.setElevatorFloor(0, 0);

            e2moveLabel.Text = "DOWN";
            e2toNumBox.Value = 0;
            e2doorLabel.Text = "CLOSE";
            this.setElevatorFloor(1, 0);

            e3moveLabel.Text = "DOWN";
            e3toNumBox.Value = 0;
            e3doorLabel.Text = "CLOSE";
            this.setElevatorFloor(2, 0);

            logTextArea.Text = "";

            this.updateElevators();
        }
    }
}
