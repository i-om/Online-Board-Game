using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ages
{
    class WorkerBank
    {
        public static int CAPACITY = 18;
        private int workersInPool = 18;
        private List<Worker> workerList = new List<Worker>();

        public int wRow = 0;
        public int wCol = 0;

        public int WorkersInPool { get { return workersInPool; } }

        public List<Worker> WorkerList {
            get { return workerList; }
            set { workerList = value; }
        }

        public int GetFoodCost() {

            int temp = 0;
            if (WorkerList.Count < 17)
            {
                temp = 4 - ((WorkerList.Count - 1) / 4);
            }
        
            return temp;
    
        }

        public int GetWorkerCost() {

            int temp;
            if (WorkerList.Count > 4)
            {
                temp = 6 - ((WorkerList.Count - 1) / 4);
            }
            else
            {
                temp = 7;
            }

            return temp;
        }

        public void AddW(Worker worker) {

            if (WorkerList.Count < CAPACITY)
            {
                if (WorkerList.Count > 0)
                    WorkerList.Last().IsPlayable = false;

                WorkerList.Add(worker);


                if (WorkerList.Count == 1)
                {
                    wCol = 1;
                    wRow = 3;
                }
                else
                {
                    if (wRow == 1)
                    {
                        wCol += 2;
                    }
                    wRow = 4 - wRow;
                }
            }
            else { throw new NotImplementedException(); }
                 
        }

        public Worker RemoveW() {

            Worker worker = WorkerList.Last();

            WorkerList.Remove(worker);

            if (WorkerList.Count > 0)
            {
                WorkerList.Last().IsPlayable = true;

                if (wRow == 3)
                {
                    wCol -= 2;
                }
                wRow = 4 - wRow;
            }
           

            return worker;
        
        }

 
    }
}
