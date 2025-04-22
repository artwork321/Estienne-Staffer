using UnityEngine;
using Ink.Runtime;
using System;
using System.Collections.Generic;

public class ConversationStack {
    private Stack<Story> conversationStack = new Stack<Story>();
    
    public Story tail => conversationStack.Peek();

    public void Push(Story story) {
        conversationStack.Push(story);
    }

    public Story Pop() {

        if (conversationStack.Count > 0) {
            return conversationStack.Pop();
        }
        
        return null;
    }

    public bool isEmpty() {
        return conversationStack.Count == 0;
    }

    public int Count() {
        return conversationStack.Count;
    }

}
