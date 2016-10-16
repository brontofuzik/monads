using System;

namespace Monads.State
{
    public class Program
    {
        public const int Indentation = 2;

        #region Tree

        public abstract class Tree<T>
        {
            public abstract void Show(int level);
        }

        public class Leaf<T> : Tree<T>
        {
            public Leaf(T content)
            {
                Content = content;
            }

            public T Content { get; set; }

            public override void Show(int level)
            {
                Console.Write(new String(' ', level * Indentation));
                Console.Write("Leaf: ");
                Content.Show(level);
                Console.WriteLine();
            }
        }

        public class Branch<T> : Tree<T>
        {
            public Branch(Tree<T> left, Tree<T> right)
            {
                Left = left;
                Right = right;
            }

            public Tree<T> Left { get; set; }

            public Tree<T> Right { get; set; }

            public override void Show(int level)
            {
                Console.Write(new String(' ', level * Indentation));
                Console.WriteLine("Branch:");
                Left.Show(level + 1);
                Right.Show(level + 1);
            }
        }

        public class TreeFactory<T>
        {
            // Unlabeled
            public Leaf<T> Leaf(T content) => new Leaf<T>(content);

            // Labeled
            public Leaf<State_Content_Pair<T>> LabeledLeaf(T content, int state)
                => new Leaf<State_Content_Pair<T>>(new State_Content_Pair<T>(state, content)); 

            // Unlabeled
            public Branch<T> Branch(Tree<T> left, Tree<T> right) => new Branch<T>(left, right);

            // Labeled
            public Branch<State_Content_Pair<T>> LabeledBranch(Tree<State_Content_Pair<T>> left, Tree<State_Content_Pair<T>> right)
                => new Branch<State_Content_Pair<T>>(left, right);
        }

        #endregion // Tree

        #region Manual labeling

        public class State_Content_Pair<T>
        {
            public State_Content_Pair(int label, T content)
            {
                Label = label;
                Content = content;
            }

            public int Label { get; set; }

            public T Content { get; set; }

            public override string ToString() => $"Label: {Label}, Contents: {Content}";
        }

        // Return first tuple
        private class LabeledTree_Label_Pair<T>
        {
            public LabeledTree_Label_Pair(Tree<State_Content_Pair<T>> labeledTree, int label)
            {           
                LabeledTree = labeledTree;
                Label = label;
            }

            public int Label { get; }

            public Tree<State_Content_Pair<T>> LabeledTree { get; }
        }

        public static Tree<State_Content_Pair<T>> Label<T>(Tree<T> tree)
        {
            var r = Label_Rec<T>(tree, 0);
            return r.LabeledTree;
        }

        private static LabeledTree_Label_Pair<T> Label_Rec<T>(Tree<T> tree, int label)
        {
            var t = new TreeFactory<T>();

            if (tree is Leaf<T>)
            {
                var leaf = (Leaf<T>)tree;
                return new LabeledTree_Label_Pair<T>(
                    t.LabeledLeaf(leaf.Content, label),
                    label + 1);
            }
            else if (tree is Branch<T>)
            {
                var branch = (Branch<T>)tree;
                var l = Label_Rec<T>(branch.Left, label);
                var r = Label_Rec<T>(branch.Right, l.Label);
                return new LabeledTree_Label_Pair<T>(
                    t.LabeledBranch(l.LabeledTree, r.LabeledTree),
                    r.Label
                );
            }
            else
            {
                throw new Exception("Lab/Label: impossible tree subtype");
            }
        }

        #endregion // Manual labeling

        #region Monadic labeling

        private static StateM<int, int> UpdateState()
            => new StateM<int, int>(n => new StateResult<int, int>(n, n + 1));

        public static Tree<State_Content_Pair<T>> MLabel<T>(Tree<T> tree)
        {
            return MLabel_Rec(tree).Func(0).Value;
        }

        private static StateM<int, Tree<State_Content_Pair<T>>> MLabel_Rec<T>(Tree<T> tree)
        {
            var t = new TreeFactory<T>();
            var s = new StateMonadFactory<int, Tree<State_Content_Pair<T>>>();

            if (tree is Leaf<T>)
            {
                var leaf = (Leaf<T>)tree;

                return UpdateState().Bind(n => s.Unit(t.LabeledLeaf(leaf.Content, n)));
            }
            else if (tree is Branch<T>)
            {
                var branch = (Branch<T>)tree;

                return MLabel_Rec(branch.Left).Bind(labeledLeft =>
                    MLabel_Rec(branch.Right).Bind(labeledRight =>
                        s.Unit(t.LabeledBranch(labeledLeft, labeledRight))));
            }
            else
            {
                throw new Exception("MakeMonad/MLabel: impossible tree subtype");
            }
        }

        #endregion // Monadic labeling

        static void Main(string[] args)
        {
            Tree<string> tree = UnlabeledTree();
            Console.WriteLine();

            HandLabeledTree();
            Console.WriteLine();

            ManuallyLabeledTree(tree);
            Console.WriteLine();

            MonadicallyLabeledTree(tree);
        }

        private static Tree<string> UnlabeledTree()
        {
            Console.WriteLine("Unlabeled Tree:");

            var t = new TreeFactory<string>();

            var tree = new Branch<string>
            (
                t.Leaf("a"),
                t.Branch
                (
                    t.Branch
                    (
                        t.Leaf("b"),
                        t.Leaf("c")
                    ),
                    t.Leaf("d")
                )
            );

            tree.Show(2);

            return tree;
        }

        private static void HandLabeledTree()
        {
            Console.WriteLine("Hand-Labeled Tree:");

            var t = new TreeFactory<State_Content_Pair<string>>();

            var tree = t.Branch
            (
                t.Leaf(new State_Content_Pair<string>(0, "a")),
                t.Branch
                (
                    t.Branch
                    (
                        t.Leaf(new State_Content_Pair<string>(1, "b")),
                        t.Leaf(new State_Content_Pair<string>(2, "c"))
                    ),
                    t.Leaf(new State_Content_Pair<string>(3, "d"))
                )
            );
            tree.Show(2);
        }

        private static void ManuallyLabeledTree(Tree<string> tree)
        {
            Console.WriteLine("Non-monadically Labeled Tree:");

            var labeledTree = Label<string>(tree);

            labeledTree.Show(2);
        }

        private static void MonadicallyLabeledTree(Tree<string> tree)
        {
            Console.WriteLine("Monadically Labeled Tree:");

            var labeledTree = MLabel<string>(tree);

            labeledTree.Show(2);
        }
    }

    public static class Extensions
    {
        public static void Show<a>(this a thing, int level)
        {
            Console.Write($"{thing}");
        }
    }
}
