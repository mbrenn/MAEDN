using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BrettSpielMeister.Model;

namespace MAEDN.Rules
{
    public class MaednMap : Map
    {
        /// <summary>
        /// Creates the map for the user
        /// </summary>
        public void Create()
        {
            var start = new FieldType();
            start.Character = 'h';

            var goal = new FieldType();
            goal.Character = 'g';

            var standard = new FieldType();
            standard.Character = 'o';



            // SetFields(fields.ToArray());

            var fields = new Field[]
            {
                new Field(0, 0, "Start red", start),
                new Field(1, 0, "Start red", start),
                new Field(0, 1, "Start red", start),
                new Field(1, 1, "Start red", start),

                new Field(9, 0, "Start yellow", start),
                new Field(10, 0, "Start yellow", start),
                new Field(9, 1, "Start yellow", start),
                new Field(10, 1, "Start yellow", start),

                new Field(0, 9, "Start green", start),
                new Field(0, 10, "Start green", start),
                new Field(1, 9, "Start green", start),
                new Field(1, 10, "Start green", start),

                new Field(9, 9, "Start blue", start),
                new Field(10, 9, "Start blue", start),
                new Field(9, 10, "Start blue", start),
                new Field(10, 10, "Start blue", start),


                new Field(1, 5, "Goal red", goal),
                new Field(2, 5, "Goal red", goal),
                new Field(3, 5, "Goal red", goal),
                new Field(4, 5, "Goal red", goal),
            
                new Field(5, 1, "Goal yellow", goal),
                new Field(5, 2, "Goal yellow", goal),
                new Field(5, 3, "Goal yellow", goal),
                new Field(5, 4, "Goal yellow", goal),

                new Field(5, 9, "Goal green", goal),
                new Field(5, 8, "Goal green", goal),
                new Field(5, 7, "Goal green", goal),
                new Field(5, 6, "Goal green", goal),

                new Field(6, 5, "Goal blue", goal),
                new Field(7, 5, "Goal blue", goal),
                new Field(8, 5, "Goal blue", goal),
                new Field(9, 5, "Goal blue", goal),

                new Field(0,4,standard),
                new Field(1,4,standard),
                new Field(2,4,standard),
                new Field(3,4,standard),
                new Field(4,4,standard),
                new Field(4,3,standard),
                new Field(4,2,standard),
                new Field(4,1,standard),
                new Field(4,0,standard),
                new Field(5,0,standard),
                new Field(6,0,standard),
                new Field(6,1,standard),
                new Field(6,2,standard),
                new Field(6,3,standard),
                new Field(6,4,standard),
                new Field(7,4,standard),
                new Field(8,4,standard),
                new Field(9,4,standard),
                new Field(10,4,standard),
                new Field(10,5,standard),
                new Field(10,6,standard),
                new Field(9,6,standard),
                new Field(8,6,standard),
                new Field(7,6,standard),
                new Field(6,6,standard),
                new Field(6,7,standard),
                new Field(6,8,standard),
                new Field(6,9,standard),
                new Field(6,10,standard),
                new Field(5,10,standard),
                new Field(4,10,standard),
                new Field(4,9,standard),
                new Field(4,8,standard),
                new Field(4,7,standard),
                new Field(4,6,standard),
                new Field(3,6,standard),
                new Field(2,6,standard),
                new Field(1,6,standard),
                new Field(0,6,standard),
                new Field(0,5,standard)
            };

            SetFields(fields);
        }
    }
}