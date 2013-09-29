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


#pragma once
#include <vector>

template <class T>
class CBuffer
{
private:
    std::vector<T> buffer;
    unsigned int size;
    unsigned int curPos;

public:
    CBuffer(unsigned int size)
    {
        this->buffer.reserve(size);
        this->size = size;
        this->curPos = 0;
    }
    ~CBuffer()
    {
        this->buffer.clear();
    }
    void add(T value)
    {
        if (this->curPos >= this->buffer.size())
            buffer.push_back(value);
        else
            buffer[this->curPos] = value;
        this->curPos = (this->curPos + 1) % this->size;
    }

    T average()
    {
        if (this->buffer.size() == 0) return 0.0f;
        T total = this->buffer[0];
        for (unsigned int i = 1; i < this->buffer.size(); i++)
        {
            total += this->buffer[i];
        }
        return total / (float)this->buffer.size();
    }
};

