using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3Dsimulator.Classes
{

    public class ETitem
    {
        public double ymax;
        public double xmin;
        public double dx_dy;
        public ETitem? next;

        public ETitem(double ymax, double xmin, double dx_dy, ETitem? next = null)
        {
            this.ymax = ymax;
            this.xmin = xmin;
            this.dx_dy = dx_dy;
            this.next = next;
        }

        public ETitem copy()
        {
            return new ETitem(ymax, xmin, dx_dy, next);
        }
    }

    public class ET_List
    {
        public ETitem head;
        public ETitem tail;

        public void Add(ETitem e)
        {
            if(head == null && tail == null)
            {
                head = tail = e;
            }
            else
            {
                tail.next = e;
                tail = e;
            }
        }

        public void Add(ETitem e, int index)
        {
            if (index == 0)
            {
                e.next = head;
                head = e;
                if (e.next == null) tail = e;
            }
            ETitem curr = head;
            for(int i=0;i<index;i++)
            {
                if(curr != null)
                    curr = curr.next;
            }
            e.next = curr;
        }
    }

}
