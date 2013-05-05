/********************************************************************************
 Copyright (C) 2012 Eric Bataille <e.c.p.bataille@gmail.com>

 This program is free software; you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation; either version 2 of the License, or
 (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with this program; if not, write to the Free Software
 Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307, USA.
********************************************************************************/

// Linked list to store currently pressed keys
struct lnode                                                
{                                                               
    int code;
    lnode *next;
};

lnode * addFront(lnode * node, int code)
{
    lnode *temp = (lnode*)malloc(sizeof(lnode));
    temp->code = code;
    temp->next = node;
    return temp;
}

void addEnd(lnode * node, int code)
{
    lnode *temp = (lnode*)malloc(sizeof(lnode));
    temp->code = code;

    lnode * cur = node;
    while(cur->next != NULL)
      cur = cur->next;

    cur->next = temp;
}

// Remove the first occurrence of a node with set code from this node
lnode * remove(lnode * node, int code)
{
    if (node->code == code)
    {
        lnode * temp = node->next;
        free(node);
        return temp;
    }

    lnode * cur = node;
    lnode * prev;
    while(cur->next != NULL && cur->code != code)
    {
        prev = cur;
        cur = cur->next;
    }

    // A node was found, remove it and update links
    if (cur->code == code)
    {
        prev->next = cur->next;
        free(cur);
    }

    return node;
}

lnode * insert(lnode * node, int code)
{
    // If there are no nodes yet, simply make one
    if (node == NULL)
        return addFront(NULL, code);
    
    lnode * cur = node;
    lnode * prev;
    while(cur->next != NULL && cur->code < code)
    {
        prev = cur;
        cur = cur->next;
    }

    // Don't insert something already there
    if (cur->code == code)
        return node;

    // We know cur's code is higher now, add it in between
    if (cur == node)
        return addFront(node, code);

    prev->next = addFront(cur, code);
    return node;
}