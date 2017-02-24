## Updating Grub2

> The best way to do this is on a Linux machine, or, as I did, using the Windows Subsystem for Linux feature available on Windows 10. Therefore, all the commands here are for a Linux host.

1. Clone the Grub2 git repository
```
~$ git clone git://git.savannah.gnu.org/grub.git
```
2. CD in the directory
```
~$ cd grub
```
3. Build Grub
```sh
~/grub$ ./autogen.sh
~/grub$ ./configure --disable-nls
~/grub$ make -j2
~/grub$ sudo make install
```
4. Create the boot image
```
~/grub$ ./grub-mkimage biosdisk iso9660 linux ntfs ext2 configfile -O i386-pc -p "/boot/grub" -o core.img
~/grub$ cat grub-core/cdboot.img core.img > eltorito.img
```
5. Replace `eltorito.img` by the newly generated one in `SharpBoot\Resources\basedisk.7z\boot\grub`.