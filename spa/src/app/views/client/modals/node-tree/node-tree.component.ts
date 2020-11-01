import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { CommentService } from 'src/app/_core/_service/comment.service';
import { IComment, ICommentTreeView } from 'src/app/_core/_model/comment.interface';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
// This is required
import { DomSanitizer } from '@angular/platform-browser';
import { CalendarsService } from 'src/app/_core/_service/calendars.service';
import { ClientRouter } from 'src/app/_core/enum/ClientRouter';
import { environment } from '../../../../../environments/environment';
import { Browser } from '@syncfusion/ej2-base';
import { ContextMenuComponent, MenuEventArgs, MenuItemModel } from '@syncfusion/ej2-angular-navigations';
import { ClipboardService } from 'ngx-clipboard';
@Component({
  selector: 'app-node-tree',
  templateUrl: './node-tree.component.html',
  styleUrls: ['./node-tree.component.css']
})
export class NodeTreeComponent implements OnInit {
  @Input() node: ICommentTreeView;
  @Input() taskID: number;
  @Input() clientRouter: ClientRouter;
  files: any;
  urls: any [];
  urlsOld: any[];
  fileList: File[] = [];
  showImageList: boolean;
  dataComment: object;
  isShow = false;
  isShowIcon = false;
  totalShow: number;
  subComment: IComment;
  content: string;
  id: number;
  usernames: any;
  userid = Number(JSON.parse(localStorage.getItem('user')).User.ID);
  mentionConfig: any;
  userID = JSON.parse(localStorage.getItem('user')).User.ID;
  galleryOptions = [
    { image: false, thumbnailsRemainingCount: true, height: '100px' },
    { breakpoint: 500, width: '100%', thumbnailsColumns: 2 }
  ];

  @ViewChild('contextmenu')
 public contextmenu: ContextMenuComponent;

 // ContextMenu items definition
 public menuItems: MenuItemModel[] ;

 // Event triggers while rendering each menu item where “Link” menu item is disabled
 public addDisabled(args: MenuEventArgs) {
     if (args.item.text === 'Link') {
         args.element.classList.add('e-disabled');
     }
 }

  constructor(
    private commentService: CommentService,
    private alertify: AlertifyService,
    private sanitizer: DomSanitizer,
    private calendar: CalendarsService,
    private clipboardService: ClipboardService
  ) { }

  ngOnInit() {
    this.isShow = false;
    this.totalShow = 3;
    this.initialParams();
    this.content = '';
    this.contextMenuOption();
  }
  contextMenuOption() {
    const currentUser = JSON.parse(localStorage.getItem('user')).User.OCLevel as number;
    if (this.node?.CreatedTaskBy === currentUser || this.node?.CreatedProjectTaskBy === currentUser) {
      this.menuItems = [
        {
            text: 'Pin',
            iconCss: 'fas fa-thumbtack'
        },
        {
            text: 'Copy',
            iconCss: 'far fa-copy'
        },
        {
            text: 'Edit',
            iconCss: 'fas fa-pen'
        },
        {
            text: 'Delete',
            iconCss: 'far fa-trash-alt'
        }
       ];
    } else {
      this.menuItems = [
        {
            text: 'Copy',
            iconCss: 'far fa-copy'
        },
        {
            text: 'Edit',
            iconCss: 'fas fa-pen'
        },
        {
            text: 'Delete',
            iconCss: 'far fa-trash-alt'
        }
       ];
    }
  }
  copy() {
    if (this.node.Images.length > 0) {
      const images = this.node.Images.map(item => {
          return environment.imagePath + item;
      });
      this.clipboardService.copyFromContent(images.join(','));
    } else {
      this.clipboardService.copyFromContent(this.node.Content);
    }
  }
  initialParams() {
    this.getUsernames();

    this.mentionConfig = {
      triggerChar: '@',
      allowSpace: true,
      maxItems: 10,
      mentionSelect($event) {
        // console.log($event);
        return `\r@${$event.label}\r `;
      }
    };
  }
  increseTotalShow() {
    this.totalShow += 3;
  }
  clickReply($event, item) {
    this.content = '';
    this.id = 0;
    if (item.Level === 2 || item.Level === 1) {
      if (this.userid !== item.UserID) {
        this.content = `\r@${item.Username}\r `;
      }
      this.isShow = !this.isShow;
    }
  }
  getUsernames() {
    this.commentService.getUsernames().subscribe(res => {
      this.usernames = res;
    });
  }
  getAllComment() {
    this.commentService.getAllComment(this.taskID, this.userid).subscribe((res: ICommentTreeView[]) => {
      this.dataComment = [];
      this.dataComment = res;
    });
  }
  checkMore() {
    return this.totalShow < this.node.children.length || this.node.children.length > this.totalShow;
  }
  addSubComment(event, parentid) {
    if (event.target.value || this.fileList) {
      // console.log('addSubComment');
      // console.log(event);
      this.subComment = {
        Content: event.target.value,
        TaskID: this.taskID,
        ParentID: parentid,
        TaskCode: '',
        ID: this.id,
        UserID: this.userid,
        ClientRouter: this.clientRouter
      };
      this.commentService.addSubComment(this.subComment).subscribe(res => {
        if (res) {
          // console.log('addSubComment: ', res);
          this.uploadImage(res);
          this.getAllComment();
          this.alertify.success('You have already added the comment successfully!');
          // this.commentService.changeMessage(200);
          this.content = '';
          this.commentService.changeMessage(200);
        } else {
          this.alertify.error('You have already added the comment failed!');
        }
      });
    } else {
      this.alertify.warning('Enter a content!', true);
    }
  }
  datetime(d) {
    return this.calendar.JSONDateWithTime(d);
  }
  imageBase64CurrentUser() {
    if (JSON.parse(localStorage.getItem('user')).User.ImageProfile === null) {
      return this.defaultImage();
    } else {
      return this.sanitizer.bypassSecurityTrustResourceUrl(
        'data:image/png;base64, ' + JSON.parse(localStorage.getItem('user')).User.image);
    }
  }
  defaultImage() {
    return '../../../../assets/img/logo-1.png';
  }
  imageBase64(img) {
    if (img == null) {
      return this.defaultImage();
    } else {
      return this.sanitizer.bypassSecurityTrustResourceUrl('data:image/png;base64, ' + img);
    }
  }

  emojiSelect($event) {
    this.content += $event.emoji.native;
  }
  onClickIcon() {
    this.isShowIcon = !this.isShowIcon;
  }
  displayImage() {
    document.getElementById('image-file-node-tre').click();
  }
  onChangeImageFile($event) {
    // console.log($event);
    this.showImageList = true;
    // this.fileList = [];
    // this.urls = [];
    this.files = $event.target.files;
    if (this.files) {
      for (let file of this.files) {
        let reader = new FileReader();
        reader.onload = (e: any) => {
          this.urls.push(e.target.result);
          this.fileList.push(file);
        };
        reader.readAsDataURL(file);
      }
      console.log('onChangeImageFile urls', this.urls);
      console.log('onChangeImageFile fileList', this.fileList);
    }
    if (this.urls.length >= 5) {
      this.urls = this.urls.slice(0, 4);
      this.alertify.warning('You have picked too many files. Limit is 5', true);
    }
  }
  toDataURL(url, callback) {
    var xhr = new XMLHttpRequest();
    xhr.onload = () => {
      var reader = new FileReader();
      reader.onloadend = () => {
        callback(reader.result);
      }
      reader.readAsDataURL(xhr.response);
    };
    xhr.open('GET', url);
    xhr.responseType = 'blob';
    xhr.send();
  }
  uploadImage(comment) {
    if (this.fileList) {
      const formData = new FormData();
      for (const iterator of this.fileList) {
        formData.append('UploadedFile', iterator);
      }
      formData.append('Comment', comment.ID);
      this.commentService.uploadImages(formData).subscribe( res => {
        // console.log(res);
        this.showImageList = false;
        this.fileList = [];
        this.urls = [];
      });
    }
  }
  removeSelectedFile(index) {
    this.fileList.splice(index, 1);
    this.urls.splice(index, 1);
    if (this.urls.length === 0) {
      this.showImageList = false;
    }
    // console.log(this.fileList);
    // console.log(this.urls);
  }
  renderGalleryImages(item) {
    let listAll = [];
    for (const iterator of item.Images) {
     let child = {
       small: environment.imagePath + iterator,
       medium: environment.imagePath + iterator,
       big: environment.imagePath + iterator
     };
     listAll.push(child);
    }
    return listAll;
   }
     // Event triggers once the context menu rendering is completed.
     onCreated(): void {
      if (Browser.isDevice) {
         // this.content = 'Touch hold to open the ContextMenu';
          this.contextmenu.animationSettings.effect = 'ZoomIn';
      } else {
         // this.content = 'Right click / Touch hold to open the ContextMenu';
          this.contextmenu.animationSettings.effect = 'SlideDown';
      }
  }
  select(args, data) {
    console.log('select', data);
    switch (args.item.text) {
      case 'Copy':
      this.copy();
      break;
      case 'Edit':
      this.edit(data);
      break;
      case 'Pin':
      this.pin();
      break;
      case 'Delete':
      this.delete();
      break;
    }
  }
  delete() {
    this.alertify.confirm(
      'Delete Comment',
      'Are you sure you want to delete this OCID "' + this.node.ID + '" ?',
      () => {
        this.commentService.delete(this.node.ID).subscribe( res => {
          if (res) {
            this.alertify.success('The comment has been deleted!!!');
            this.commentService.changeMessage(200);
          } else {
            this.alertify.error('Failed to delete the comment!!!');
          }
        });
      }
    );
  }
  pin() {
    this.alertify.confirm(
      'Pin this message',
      'Set this message as a notice and notify all group members?',
      () => {
        this.commentService.pin(this.node.ID, this.taskID, this.userID).subscribe( res => {
          if (res) {
            this.alertify.success('You pinned a message!!!');
            this.commentService.changeMessage(200);
          } else {
            this.alertify.error('Failed to pin the message!!!');
          }
        });
      }
    );
  }
  edit(data) {
    this.isShow = !this.isShow;
    this.content = this.node.Content;
    this.id = this.node.ID;
  }
}
