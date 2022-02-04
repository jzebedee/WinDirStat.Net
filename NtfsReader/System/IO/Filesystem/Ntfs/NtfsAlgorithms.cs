/*
    The NtfsReader library.

    Copyright (C) 2008 Danny Couture

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public
    License as published by the Free Software Foundation; either
    version 2.1 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public
    License along with this library; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
  
    For the full text of the license see the "License.txt" file.

    This library is based on the work of Jeroen Kessels, Author of JkDefrag.
    http://www.kessels.com/Jkdefrag/
    
    Special thanks goes to him.
  
    Danny Couture
    Software Architect
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace System.IO.Filesystem.Ntfs;

public static class NtfsAlgorithms
{
    public static IDictionary<uint, List<INtfsNode>> AggregateByFragments(IEnumerable<INtfsNode> nodes, uint minimumFragments)
    {
        Dictionary<uint, List<INtfsNode>> fragmentsAggregate = new();

        foreach (INtfsNode node in nodes)
        {
            IList<INtfsStream> streams = node.Streams;
            if (streams == null || streams.Count == 0)
            {
                continue;
            }

            IList<INtfsFragment> fragments = streams[0].Fragments;
            if (fragments == null)
            {
                continue;
            }

            uint fragmentCount = (uint)fragments.Count;

            if (fragmentCount < minimumFragments)
            {
                continue;
            }

            fragmentsAggregate.TryGetValue(fragmentCount, out List<INtfsNode> nodeList);

            if (nodeList == null)
            {
                nodeList = new List<INtfsNode>();
                fragmentsAggregate[fragmentCount] = nodeList;
            }

            nodeList.Add(node);
        }

        return fragmentsAggregate;
    }

    public static IDictionary<ulong, List<INtfsNode>> AggregateBySize(IEnumerable<INtfsNode> nodes, ulong minimumSize)
    {
        Dictionary<ulong, List<INtfsNode>> sizeAggregate = new();

        foreach (INtfsNode node in nodes)
        {
            if ((node.Attributes & FileAttributes.Directory) != 0 || node.Size < minimumSize)
            {
                continue;
            }

            sizeAggregate.TryGetValue(node.Size, out List<INtfsNode> nodeList);

            if (nodeList == null)
            {
                nodeList = new List<INtfsNode>();
                sizeAggregate[node.Size] = nodeList;
            }

            nodeList.Add(node);
        }

        return sizeAggregate;
    }
}
