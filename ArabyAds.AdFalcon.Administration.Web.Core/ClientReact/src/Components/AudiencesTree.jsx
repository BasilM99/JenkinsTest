import React, { Component } from 'react';
import Tree, { TreeNode } from 'rc-tree';
import axios from 'axios';
import { gData } from "./util";
import ReactDOM from "react-dom";
import BaseComponent from './Base/BaseComponent';
import Autosuggest from 'react-autosuggest';
import Tooltip from "rc-tooltip";
//import '../../content/styles/bootstrap_white.css';

function switcherIcon(obj) {
  if (obj.isLeaf) {
    return "";
  }
  return (
    <i className="rc-tree-switcher-icon">
      <svg
        viewBox="0 0 1024 1024"
        width="1em"
        height="1em"
        fill="currentColor"
        aria-hidden="true"
      >
        <path d="M840.4 300H183.6c-19.7 0-30.7 20.8-18.5 35l328.4 380.8c9.4 10.9 27.5 10.9 37 0L858.9 335c12.2-14.2 1.2-35-18.5-35z" />
      </svg>
    </i>
  );
}

class Demo extends React.Component {
  state = {
    gData,
    autoExpandParent: true,
    expandedKeys: ["0-0-key", "0-0-0-key", "0-0-0-0-key"],
    treeData:[]
    
  };

  componentDidMount(){
    
		axios
		.request({
			url:  "/en/Tree/Get?type=2&factId=1&IncludeId=false&_=1606228546490",
			method: 'GET',
		})
		.then(res =>{
			//debugger;
			this.state.treeData = res.data;
			this.setState({treeData});

		})
		.catch(err => console.log('error', err));

  }
  onDragStart = info => {
    console.log("start", info);
  };

  onDragEnter = info => {
    console.log("enter", info);
    this.setState({
      expandedKeys: info.expandedKeys
    });
  };

  onDrop = info => {
    console.log("drop", info);
    const dropKey = info.node.props.eventKey;
    const dragKey = info.dragNode.props.eventKey;
    const dropPos = info.node.props.pos.split("-");
    const dropPosition =
      info.dropPosition - Number(dropPos[dropPos.length - 1]);

    const loop = (data, key, callback) => {
      data.forEach((item, index, arr) => {
        if (item.key === key) {
          callback(item, index, arr);
          return;
        }
        if (item.children) {
          loop(item.children, key, callback);
        }
      });
    };
    const data = [...this.state.gData];

    // Find dragObject
    let dragObj;
    loop(data, dragKey, (item, index, arr) => {
      arr.splice(index, 1);
      dragObj = item;
    });

    if (!info.dropToGap) {
      // Drop on the content
      loop(data, dropKey, item => {
        item.children = item.children || [];
        // where to insert 示例添加到尾部，可以是随意位置
        item.children.push(dragObj);
      });
    } else if (
      (info.node.props.children || []).length > 0 && // Has children
      info.node.props.expanded && // Is expanded
      dropPosition === 1 // On the bottom gap
    ) {
      loop(data, dropKey, item => {
        item.children = item.children || [];
        // where to insert 示例添加到尾部，可以是随意位置
        item.children.unshift(dragObj);
      });
    } else {
      // Drop on the gap
      let ar;
      let i;
      loop(data, dropKey, (item, index, arr) => {
        ar = arr;
        i = index;
      });
      if (dropPosition === -1) {
        ar.splice(i, 0, dragObj);
      } else {
        ar.splice(i + 1, 0, dragObj);
      }
    }

    this.setState({
      gData: data
    });
  };

  onExpand = expandedKeys => {
    console.log("onExpand", expandedKeys);
    this.setState({
      expandedKeys,
      autoExpandParent: false
    });
  };

  render() {
    const loop = data =>
      data.map(item => {
        if (item.children && item.children.length) {
          return (
            <TreeNode key={item.key} title={item.title}>
              {loop(item.children)}
            </TreeNode>
          );
        }
        return <TreeNode key={item.key} title={item.title} />;
      });
    return (
      <div className="draggable-demo">
        <h2>draggable</h2>
        <p>drag a node into another node</p>
        <div className="draggable-container">
          <Tree
            expandedKeys={this.state.expandedKeys}
            onExpand={this.onExpand}
            autoExpandParent={this.state.autoExpandParent}
            draggable
            onDragStart={this.onDragStart}
            onDragEnter={this.onDragEnter}
            onDrop={this.onDrop}
            switcherIcon={switcherIcon}
          >
            {loop(this.state.treeData)}
          </Tree>
        </div>
      </div>
    );
  }
}

export default Demo;