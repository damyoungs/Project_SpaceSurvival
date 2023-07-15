

using System;

public sealed class TurnLinkedListNode<T> where T : class
{
    private TurnLinkedListNode<T> prevNode;
    public TurnLinkedListNode<T> Previous => prevNode;
    

    private TurnLinkedListNode<T> nextNode;
    public TurnLinkedListNode<T> NextNode => nextNode;


    private TurnLinkedList<T> list;
    public TurnLinkedList<T> List => list;





}
