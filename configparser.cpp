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


#include "configparser.h"
#include <iostream>
#include <fstream>

using std::string; using std::map;

ConfigParser::ConfigParser(LPSTR filename)
{
    // Load some defaults
    config["keyboardFile"] = "us_intl.kb";
    config["backR"] = "0";
    config["backG"] = "0";
    config["backB"] = "100";
    config["pressedR"] = "255";
    config["pressedG"] = "255";
    config["pressedB"] = "255";
    config["looseR"] = "100";
    config["looseG"] = "100";
    config["looseB"] = "100";
    config["fontR"] = "0";
    config["fontG"] = "0";
    config["fontB"] = "0";

    // Read the general settings
    string word;
    string value;
    std::ifstream is(filename);
    while (is.good() && is)
    {
        is >> word >> value;
        config[word] = value;
    }
}

void ConfigParser::SaveSettings(LPSTR filename)
{
  std::ofstream os;
  os.open(filename, std::ios::trunc);
  map<string, string>::iterator curr,end;
  for( curr = config.begin(), end = config.end();  curr != end;  curr++ )
      os << curr->first << " " << curr->second <<  "\n";
  os.close();
}

bool ConfigParser::HasItem(string word)
{
    return config.find(word) != config.end();
}

int ConfigParser::GetInt(string word)
{
    if (!HasItem(word)) return -1;
    string s = config[word];
    return atoi(s.c_str()); 
}

string ConfigParser::GetString(string word)
{
    if (!HasItem(word)) return "";
    return config[word];
}
