using System.Collections.Generic;
using BrettSpielMeister.Model;

namespace MAEDN.Rules
{
    public class MaednMap : Map
    {
        public IEnumerable<Field> RedHomeFields => new[]
        {
            Fields![0],
            Fields![1],
            Fields![2],
            Fields![3]
        };

        public IEnumerable<Field> YellowHomeFields => new[]
        {
            Fields![4],
            Fields![5],
            Fields![6],
            Fields![7]
        };

        public IEnumerable<Field> GreenHomeFields => new[]
        {
            Fields![8],
            Fields![9],
            Fields![10],
            Fields![11]
        };

        public IEnumerable<Field> BlueHomeFields => new[]
        {
            Fields![12],
            Fields![13],
            Fields![14],
            Fields![15]
        };

        public IEnumerable<Field> RedGoalFields => new[]
        {
            Fields![16],
            Fields![17],
            Fields![18],
            Fields![19]
        };

        public IEnumerable<Field> YellowGoalFields => new[]
        {
            Fields![20],
            Fields![21],
            Fields![22],
            Fields![23]
        };

        public IEnumerable<Field> GreenGoalFields => new[]
        {
            Fields![24],
            Fields![25],
            Fields![26],
            Fields![27]
        };

        public IEnumerable<Field> BlueGoalFields => new[]
        {
            Fields![28],
            Fields![29],
            Fields![30],
            Fields![31]
        };

        public Field RedStartField => Fields![32];

        public Field YellowStartField => Fields![42];

        public Field GreenStartField => Fields![52];

        public Field BlueStartField => Fields![62];

        /// <summary>
        /// Gets the fields on which the figures are moving
        /// </summary>
        public IEnumerable<Field> MovingFields
        {
            get
            {
                for (var n = 32; n <= 71; n++)
                {
                    yield return Fields![n];
                }
            }
        }

        /// <summary>
        ///     Creates the map for the user
        /// </summary>
        public override void Create()
        {
            var start = new FieldType {Character = 'h'};

            var goal = new FieldType {Character = 'g'};

            var standard = new FieldType {Character = 'o'};

            var fields = new[]
            {
                new Field(0, 0, "Start red", start), // 0
                new Field(1, 0, "Start red", start), // 1
                new Field(0, 1, "Start red", start), // 2
                new Field(1, 1, "Start red", start), // 3

                new Field(9, 0, "Start yellow", start),  // 4
                new Field(10, 0, "Start yellow", start), // 5
                new Field(9, 1, "Start yellow", start),  // 6
                new Field(10, 1, "Start yellow", start), // 7

                new Field(0, 9, "Start green", start),  // 8
                new Field(0, 10, "Start green", start), // 9 
                new Field(1, 9, "Start green", start),  // 10
                new Field(1, 10, "Start green", start), // 11

                new Field(9, 9, "Start blue", start),   // 12
                new Field(10, 9, "Start blue", start),  // 13
                new Field(9, 10, "Start blue", start),  // 14
                new Field(10, 10, "Start blue", start), // 15

                new Field(1, 5, "Goal red 1", goal),  // 16
                new Field(2, 5, "Goal red 2", goal),  // 17
                new Field(3, 5, "Goal red 3", goal),  // 18
                new Field(4, 5, "Goal red 4", goal),  // 19
                
                new Field(5, 1, "Goal yellow 1", goal), // 20
                new Field(5, 2, "Goal yellow 2", goal), // 21
                new Field(5, 3, "Goal yellow 3", goal), // 22
                new Field(5, 4, "Goal yellow 4", goal), // 23

                new Field(5, 9, "Goal green 1", goal), // 24
                new Field(5, 8, "Goal green 2", goal), // 25
                new Field(5, 7, "Goal green 3", goal), // 26
                new Field(5, 6, "Goal green 4", goal), // 27

                new Field(9, 5, "Goal blue 1", goal),  // 28
                new Field(8, 5, "Goal blue 2", goal),  // 29
                new Field(7, 5, "Goal blue 3", goal),  // 30
                new Field(6, 5, "Goal blue 4", goal),  // 31

                new Field(0, 4, "Red 0", standard), // 32, red block
                new Field(1, 4, "Red 1", standard), // 33
                new Field(2, 4, "Red 2", standard), // 34
                new Field(3, 4, "Red 3", standard), // 35
                new Field(4, 4, "Red 4", standard), // 36
                new Field(4, 3, "Red 5", standard), // 37
                new Field(4, 2, "Red 6", standard), // 38
                new Field(4, 1, "Red 7", standard), // 39
                new Field(4, 0, "Red 8", standard), // 40
                new Field(5, 0, "Red 9", standard), // 41, yellow last
                new Field(6, 0, "Yellow 0", standard), // 42, yellow block
                new Field(6, 1, "Yellow 1", standard), // 43
                new Field(6, 2, "Yellow 2", standard), // 44
                new Field(6, 3, "Yellow 3", standard), // 45
                new Field(6, 4, "Yellow 4", standard), // 46
                new Field(7, 4, "Yellow 5", standard), // 47
                new Field(8, 4, "Yellow 6", standard), // 48
                new Field(9, 4, "Yellow 7", standard), // 49
                new Field(10, 4, "Yellow 8", standard),// 50
                new Field(10, 5, "Yellow 9", standard),// 51, green last
                new Field(10, 6, "Green 0", standard),// 52, green out
                new Field(9, 6, "Green 1", standard), // 53
                new Field(8, 6, "Green 2", standard), // 54
                new Field(7, 6, "Green 3", standard), // 55
                new Field(6, 6, "Green 4", standard), // 56
                new Field(6, 7, "Green 5", standard), // 57
                new Field(6, 8, "Green 6", standard), // 58
                new Field(6, 9, "Green 7", standard), // 59
                new Field(6, 10, "Green 8", standard),// 60
                new Field(5, 10, "Green 9", standard),// 61, blue last
                new Field(4, 10, "Blue 0", standard),// 62, blue out
                new Field(4, 9, "Blue 1", standard), // 63
                new Field(4, 8, "Blue 2", standard), // 64
                new Field(4, 7, "Blue 3", standard), // 65
                new Field(4, 6, "Blue 4", standard), // 66
                new Field(3, 6, "Blue 5", standard), // 67
                new Field(2, 6, "Blue 6", standard), // 68
                new Field(1, 6, "Blue 7", standard), // 69
                new Field(0, 6, "Blue 8", standard), // 70
                new Field(0, 5, "Blue 9", standard)  // 71, red last
            };

            SetFields(fields);
        }
    }
}